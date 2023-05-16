using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MosOilConn.Entities;
using static Org.BouncyCastle.Math.EC.ECCurve;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MosOilConn
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppFuncController : Controller
    {
        private readonly TestdbContext testdbContext;
        private readonly IConfiguration _config;
        private static Random _random = new Random();
        private static List<int> _generatedNumbers = new List<int>();

        // GET: /<controller>/
        public AppFuncController(TestdbContext TestdbContext, IConfiguration config)
        {
            _config = config;
            this.testdbContext = TestdbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCardAndVerifyToken([FromBody] AppDTO appDTO)
        {
            string token = appDTO.token;
            int userId = appDTO.userId;
           //  if (!VerifyToken(token, userId))
           // {
             // return BadRequest("Неверный токен или идентификатор пользователя.");
            //}
            var user = await testdbContext.Users.SingleOrDefaultAsync(u => u.IdUser == userId);

            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }
            var bonuscard = await testdbContext.Bonuscards
                .Include(bc => bc.Users)
                .FirstOrDefaultAsync(bc => bc.Users.Any(user => user.IdUser == userId));

            if (bonuscard != null)
            {
                bonuscard.numberOfQR = GenerateUniqueRandomNumberWithUserId(userId);

                // Сохраните изменения в базе данных
                await testdbContext.SaveChangesAsync();

                int scoresCard = bonuscard.ScoresCard;
                int numberOfQR = bonuscard.numberOfQR;

                // Верните scoresCard и numberOfQR в ответе
                return Ok(new { scoresCard, numberOfQR });
            }
            else
            {

                // Привязать карту к пользователю

                var card = await testdbContext.Bonuscards
                       .FirstOrDefaultAsync(bc => bc.Users.Any(u => u.IdUser == userId));

                if (card != null)
                {
                    // Обновить номер QR
                    card.numberOfQR = GenerateUniqueRandomNumberWithUserId(userId);
                }
                else
                {
                    // Создать новую карту
                    card = new Bonuscard
                    {
                        ScoresCard = 0,
                        numberOfQR = GenerateUniqueRandomNumberWithUserId(userId)
                    };

                    // Привязать карту к пользователю
                    user.IdCardNavigation = card;
                }

                // Сохранить изменения в базе данных
                await testdbContext.SaveChangesAsync();

                int scoresCard = card.ScoresCard;
                int numberOfQR = card.numberOfQR;

                // Вернуть scoresCard и numberOfQR в ответе
                return Ok(new { scoresCard, numberOfQR });
            }
        }
            private static int GenerateUniqueRandomNumberWithUserId(int userId)
        {
            int randomNumber;
            int minNumber = 10000000;
            int maxNumber = 99999999;

            do
            {
                int randomSuffix = _random.Next(minNumber, maxNumber); // Генерация случайного числа для добавления в конец
                string randomNumberString = $"{userId}{randomSuffix}"; // Соединение идентификатора пользователя и случайного числа
                randomNumber = int.Parse(randomNumberString);
            }
            while (_generatedNumbers.Contains(randomNumber));

            _generatedNumbers.Add(randomNumber);

            return randomNumber;
        }
        private bool VerifyToken(string token, int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:SecretKey"]);

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken validatedToken;
                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                if (validatedToken == null || !claimsPrincipal.Identity.IsAuthenticated)
                {
                    return false;
                }

                var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var tokenUserId))
                {
                    return false;
                }

                return userId == tokenUserId;
            }
            catch
            {
                return false;
            }
        }

    }
}


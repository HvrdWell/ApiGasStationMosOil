using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MosOilConn.Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MosOilConn
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExolveSMSAuthController : Controller
    {
        private readonly TestdbContext TestdbContext;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly string _exolveApiUrl;
        private readonly string _exolveApiKey;

        public ExolveSMSAuthController(IConfiguration config, TestdbContext TestdbContext)
        {
            this.TestdbContext = TestdbContext;
            _httpClient = new HttpClient();
            _config = config;
            _exolveApiUrl = "https://api.exolve.ru/messaging/v1/SendSMS";
            _exolveApiKey = ".eyJleHAiOjE5OTkxMTA0MjMsImlhdCI6MTY4Mzc1MDQyMywianRpIjoiNTMyMjA3ZjUtMDYzOS00ZmEzLWExNTYtMzNjZDk2YjhiNzcwIiwiaXNzIjoiaHR0cHM6Ly9zc28uZXhvbHZlLnJ1L3JlYWxtcy9FeG9sdmUiLCJhdWQiOiJhY2NvdW50Iiwic3ViIjoiMjE5N2JiZGYtNTAyNC00MTgxLTlhODgtNzFjY2Y5MzMwNDlmIiwidHlwIjoiQmVhcmVyIiwiYXpwIjoiNzZjYjVmZGUtOWY2Zi00ZmI2LTg4MmEtYzczMmY4MGI5NjIxIiwic2Vzc2lvbl9zdGF0ZSI6IjY5ZDNhZmFkLTUyNTEtNDg5YS05NDNjLTY2ZWJiODU1OGY5NiIsImFjciI6IjEiLCJyZWFsbV9hY2Nlc3MiOnsicm9sZXMiOlsiZGVmYXVsdC1yb2xlcy1leG9sdmUiLCJvZmZsaW5lX2FjY2VzcyIsInVtYV9hdXRob3JpemF0aW9uIl19LCJyZXNvdXJjZV9hY2Nlc3MiOnsiYWNjb3VudCI6eyJyb2xlcyI6WyJtYW5hZ2UtYWNjb3VudCIsIm1hbmFnZS1hY2NvdW50LWxpbmtzIiwidmlldy1wcm9maWxlIl19fSwic2NvcGUiOiJleG9sdmVfYXBwIHByb2ZpbGUgZW1haWwiLCJzaWQiOiI2OWQzYWZhZC01MjUxLTQ4OWEtOTQzYy02NmViYjg1NThmOTYiLCJ1c2VyX3V1aWQiOiIxNjg3ZDA5YS1mYzc2LTQ3NDYtOGJiNi02MGUyMWIxN2JmNjEiLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImNsaWVudEhvc3QiOiIxNzIuMjAuMi4yMSIsImNsaWVudElkIjoiNzZjYjVmZGUtOWY2Zi00ZmI2LTg4MmEtYzczMmY4MGI5NjIxIiwiYXBpX2tleSI6dHJ1ZSwiYXBpZm9uaWNhX3NpZCI6Ijc2Y2I1ZmRlLTlmNmYtNGZiNi04ODJhLWM3MzJmODBiOTYyMSIsImJpbGxpbmdfbnVtYmVyIjoiMTE5NjMxNyIsImFwaWZvbmljYV90b2tlbiI6ImF1dDQ4OWFmNDI3LTNhNGUtNDY4NS1iNjAxLWI4ZTFhMTgwNTg1YSIsInByZWZlcnJlZF91c2VybmFtZSI6InNlcnZpY2UtYWNjb3VudC03NmNiNWZkZS05ZjZmLTRmYjYtODgyYS1jNzMyZjgwYjk2MjEiLCJjdXN0b21lcl9pZCI6IjI2MDI3IiwiY2xpZW50QWRkcmVzcyI6IjE3Mi4yMC4yLjIxIn0.EG4ksoa86HVB6o8gduT6oqVYLmOA0k62Wkvozfl2uEFUACFfjxvfiWF5ivXrSRjebqJghig3HHZJJJUce_R3Xzi9P2lke6eH1_mRNssZdwka72wbBLjcTeboqXB3Motpj7z8lpShtKRt20uBrN8OdWkTWk0OvJx46XwD1SvEHDqa2neFOdVISYkXlICYJSnA_Py08b1cF7SmPrJNFoHwYwUV6bak2nPOmkhXBemJcc_7TJzt9vPXyxsbgn37Qv4a7ZVf7kgV1vjI1Ol3faRrsILdE3jwgOTVt8S4EJvx3uB7sUVmFzkwU33cUImud5Z4Y2yIMJe1HqLZFjQEAQUEpw"; 
        }

        // Метод для отправки SMS-кода
        private async Task<bool> SendSMSCode(string phoneNumber, string verificationCode)
        {
            var requestData = new
            {
                number = "79668893729", //отправитель Exolve
                destination = phoneNumber,
                text = verificationCode
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _exolveApiKey);

            var response = await _httpClient.PostAsync(_exolveApiUrl, content);

            return response.IsSuccessStatusCode;
        }

        // POST: /api/ExolveSMSAuth/SendCode
        [HttpPost("SendCode")]
        public async Task<IActionResult> SendCode([FromBody] string phoneNumber)
        {
            // Генерация кода подтверждения
            var verificationCode = GenerateVerificationCode();

            // Отправка SMS-кода
            var isSMSSent = await SendSMSCode(phoneNumber, verificationCode);
            if (isSMSSent)
            {
                SaveVerificationCode(phoneNumber, verificationCode);
                return Ok();
            }
            else
            {
                return StatusCode(500, "Failed to send SMS");
            }
        }

        [HttpPost("asd")]
        public IActionResult asd(string phoneNumber)
        {
         
            return Ok();
        }

        // Метод для генерации кода подтверждения
        private string GenerateVerificationCode()
        {
            Random random = new Random();
            int randomNumber = random.Next(100, 999);
            return $"{randomNumber}"; 
        }
        // Метод для сохранения кода подтверждения
        private void SaveVerificationCode(string phoneNumber, string verificationCode)
        {
            // сохранение в сессии:
            HttpContext.Session.SetString(phoneNumber, verificationCode);
        }

        private string GenerateJwtToken(int userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                // Add additional claims as needed
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private int GetUserId(string phoneNumber)
        {
            using (var context = new TestdbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
                if (user != null)
                {
                    return user.IdUser; 
                }
                else
                {
                    return 0; 
                }
            }
        }

        [HttpPost("VerifyCode")]
        public ActionResult<TokenResponse> VerifyCode([FromBody] verifyCodeModel model)
        {
            string phoneNumber = model.PhoneNumber;
            string code = model.Code;

            // Получение сохраненного кода подтверждения из сессии
            var savedCode = HttpContext.Session.GetString(phoneNumber);

            if (savedCode == code)
            {
                // Код подтверждения верный

                // Здесь можно выполнить дополнительные действия после успешной авторизации, например, создать токен доступа и получить идентификатор пользователя
                int userId = GetUserId(phoneNumber); // Получение идентификатора пользователя

                string token = GenerateJwtToken(userId); // Генерация токена доступа

                TokenResponse response = new TokenResponse
                {
                    Token = token,
                    UserId = userId
                };

                return Ok(response);
            }
            else
            {
                // Код подтверждения неверный
                return Unauthorized();
            }
        }

    }
}

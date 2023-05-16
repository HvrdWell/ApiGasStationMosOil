using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MosOilConn.Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MosOilConn
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TestdbContext _dbContext;
        private readonly IConfiguration _config;

        public AuthController(TestdbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO userDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == userDto.Login);

            if (user == null || !VerifyPassword(userDto.Password, user.Password))
            {
                return BadRequest("Invalid username or password");
            }


            var token = GenerateJwtToken(user);

            var random = new Random();

            if (user.IdCardNavigation == null)
            {
                var newCard = new Bonuscard
                {
                    IdCard = 124,
                    ScoresCard = 0,
                    numberOfQR = random.Next(100, 200)
                };

                user.IdCardNavigation = newCard;
            }
            else
            {
                user.IdCardNavigation.numberOfQR = random.Next(100, 200);
            }

            await _dbContext.SaveChangesAsync();

            var userResponse = new
            {
                Token = token,
                User = new
                {
                    user.IdUser,
                
                }
            };

            return Ok(userResponse);
        }
        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {

            return enteredPassword == storedPassword;
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
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
    }
}


using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MosOilConn.Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MosOilConn.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly TestdbContext TestdbContext;
        private readonly IConfiguration _config;

        private string GenerateKey()
        {
            return "your_secret_key_here";
        }
        // GET: /<controller>/
        public UserController(TestdbContext TestdbContext, IConfiguration config)
        {
            this.TestdbContext = TestdbContext;
            _config = config;
        }




      


        [HttpGet("GetUserById")]
        public async Task<ActionResult<UserDTO>> GetUserById(int idUser)
        {
            UserDTO User = await TestdbContext.Users.Select(
                    s => new UserDTO
                    {
                        idUser = s.IdUser,
                       // idCard = s.IdCard,
                        phoneNumber = s.PhoneNumber,
                        role = s.Role

                    })
                .FirstOrDefaultAsync(s => s.idUser == idUser);

            if (User == null)
            {
                return NotFound();
            }
            else
            {
                return User;
            }
        }
    }
}


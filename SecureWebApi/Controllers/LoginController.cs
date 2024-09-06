using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecureWebApi.Context;
using SecureWebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SecureWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        AppDbContext _repo;
        IConfiguration _config;
        public LoginController(AppDbContext repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }
        //[HttpPost]
        //public IActionResult Login(User user)
        //{
        //    User obj = null;
        //    foreach (var temp in _repo.Uers)
        //    {
        //        if (user.username == temp.username && user.password == temp.password)
        //        {
        //            return Ok(temp);
        //        }
        //    }
        //    return NotFound();

        //}

        [HttpPost]
        public IActionResult Login(User user)
        {
            IActionResult response = Unauthorized();
            var obj = Authenticate(user);
            if (obj != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }
            return response;


        }


        private string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User Authenticate(User user)
        {   foreach (var temp in _repo.Uers)
            {
                if (user.username == temp.username && user.password == temp.password)
                {
                    return (temp);
                }
            }
            return null;

        }


    }
}
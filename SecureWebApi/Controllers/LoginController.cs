using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecureWebApi.Context;
using SecureWebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
                var tokenString = GenerateJSONWebToken(obj);
                response = Ok(new { token = tokenString });
            }
            return response;


        }

        private string GetRoleName(int roleId)
        {
            string roleName = ( from x in _repo.Roles
                                where x.RoleId==roleId
                                select x.RoleName).FirstOrDefault();
            return roleName;
        }

        private string GenerateJSONWebToken(User user)
        {
            string role = GetRoleName(user.RoleId);

            Claim[] claims = new[]
         {
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
                 new Claim(JwtRegisteredClaimNames.Name, user.username),
                 new Claim("Role", role.ToString()),
                 new Claim(type:"Date", DateTime.Now.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
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
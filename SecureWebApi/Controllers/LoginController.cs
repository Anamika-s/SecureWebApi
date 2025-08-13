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

            // Assuming 'userRoles' is a collection of strings representing the user's roles
            //foreach (var role in userRoles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role));
            //}
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role,role.ToString())
    };


             
            var claimsIdentity = new ClaimsIdentity(claims, "JWT"); // "JWT" is the authentication type
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            foreach (var temp in _repo.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, temp.RoleName));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {   Audience = _config["Jwt:Issuer"],
                Expires = DateTime.Now.AddMinutes(120),
                Issuer = _config["Jwt:Issuer"],
                Subject = claimsPrincipal.Identity as ClaimsIdentity,

                
                SigningCredentials =credentials
                  
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string jwtToken = tokenHandler.WriteToken(token);
            return jwtToken;
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
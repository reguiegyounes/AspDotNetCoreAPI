using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using projectApi.Models;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace projectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration; 
        private readonly dbBooksEssaContext _context;

        public TokenController(IConfiguration config, dbBooksEssaContext context)
        {
            _configuration = config;
            _context = context;
        }


        [HttpPost]
        public async Task<ActionResult> PostUser(TblUser userData)
        {
            if(userData!=null && userData.Email!=null && userData.Password!=null)
            {
                var user = await GetUser(userData.Email, userData.Password);
                if (user !=null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                        new Claim("id",user.UserId.ToString()),
                        new Claim("firstName",user.FirstName),
                        new Claim("lastName",user.LastName),
                        new Claim("username",user.Username),
                        new Claim("email",user.Email)
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires:DateTime.UtcNow.AddDays(1),
                        signingCredentials:signIn
                        );

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));

                }
                else
                {
                    return BadRequest("Invalid email and password");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<TblUser> GetUser(string email, string password)
        {
            return await _context.TblUsers.FirstOrDefaultAsync(u => u.Email==email && u.Password==password );
        }
    }
}

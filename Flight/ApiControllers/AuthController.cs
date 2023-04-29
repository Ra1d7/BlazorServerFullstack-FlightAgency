using FlightAgency.Data;
using FlightAgency.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Flight.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly FlightDB _db;

        public record UserData(string email, string password);
        public AuthController(IConfiguration config, FlightDB db)
        {
            _config = config;
            _db = db;
        }
        [HttpPost]
        [Route("CreateToken")]
        [AllowAnonymous]
        public IActionResult GetToken([FromBody] UserData data)
        {
            var user = CheckData(data);
            return (user is null) ? Unauthorized() : Ok(GenToken(user));
        }

        private User? CheckData(UserData data)
        {
            var user = data.email;
            var hash = Convert.ToHexString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(data.password)));
            return null;
        }
        private string? GenToken(User user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("Authentication:SecretKey")));
            var SigningCreds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new();
            claims.Add(new(JwtRegisteredClaimNames.Sub, user.Username));
            claims.Add(new Claim("Role", user.Role.ToString()));
            var token = new JwtSecurityToken(
                _config.GetValue<string>("Authentication:Issuer"),
                _config.GetValue<string>("Authentication:Audience"),
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(1),
                SigningCreds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

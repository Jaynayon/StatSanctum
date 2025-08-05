using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace StatSanctum.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration _configuration;
        private const int _hours = 1;
        private static string _key = string.Empty;
        private static string _authority = string.Empty;
        private static string _audience = string.Empty;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _key = _configuration["Authentication:SecretKey"] ?? throw new ArgumentNullException("Security key is empty");
            _authority = _configuration["Authentication:Authority"] ?? throw new ArgumentNullException("Authority is empty");
            _audience = _configuration["Authentication:Audience"] ?? throw new ArgumentNullException("Audience is empty");
        }

        [HttpGet("token")]
        public ActionResult<string> GetToken()
        {
            // validate user credentials

            // create a token
            var token = GenerateToken(_key, _authority, _audience);

            return Ok(token);
        }

        private static string GenerateToken(string key, string authority, string audience)
        {
            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(_key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("sub", "basta"),
                new Claim("username", "basta username"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())  // Unique token ID
            };

            var token = new JwtSecurityToken(
                _authority,
                _audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(_hours),
                signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

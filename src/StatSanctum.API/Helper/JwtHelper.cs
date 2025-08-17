using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StatSanctum.API.Helper
{
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration _configuration;
        private const int _hours = 1;
        private string _key = string.Empty;
        private string _authority = string.Empty;
        private string _audience = string.Empty;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _key = _configuration["Authentication:SecretKey"] ?? throw new ArgumentNullException("Security key is empty");
            _authority = _configuration["Authentication:Authority"] ?? throw new ArgumentNullException("Authority is empty");
            _audience = _configuration["Authentication:Audience"] ?? throw new ArgumentNullException("Audience is empty");
        }

        public string GenerateToken(string sub, string name)
        {
            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(_key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("sub", sub),
                new Claim("name", name),
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

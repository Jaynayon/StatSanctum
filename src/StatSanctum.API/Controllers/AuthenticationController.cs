using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StatSanctum.API.Enums;
using StatSanctum.API.Models;
using StatSanctum.API.Queries.Users;
using StatSanctum.Entities;
using StatSanctum.Handlers;

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

        private IMediator _mediator;
        private IMapper _mapper;

        public AuthenticationController(IConfiguration configuration, IMediator mediator, IMapper mapper)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _key = _configuration["Authentication:SecretKey"] ?? throw new ArgumentNullException("Security key is empty");
            _authority = _configuration["Authentication:Authority"] ?? throw new ArgumentNullException("Authority is empty");
            _audience = _configuration["Authentication:Audience"] ?? throw new ArgumentNullException("Audience is empty");

            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        //[HttpGet("token")]
        //public ActionResult<string> GetToken()
        //{
        //    // validate user credentials

        //    // create a token
        //    var token = GenerateToken("", "");

        //    return Ok(token);
        //}

        [HttpPost("login")]
        public async Task<ActionResult<string>> VerifyUser(UserLoginDto request)
        {
            if (request == null)
            {
                return BadRequest("User data is required.");
            }
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Username and password are required.");
            }
            try
            {
                var (userId, username) = await _mediator.Send(new ValidateUserCommand<User>
                {
                    Username = request.Username,
                    Password = request.Password,
                    Method = AuthenticationMethod.Manual
                });

                if (userId != 0 && !string.IsNullOrWhiteSpace(username))
                {
                    var token = GenerateToken(Convert.ToString(userId), username);
                    return Ok(token); // Login successful
                }

                return Unauthorized("Invalid username or password."); // Login failed
            }
            catch(ArgumentException)
            {
                return Unauthorized("Invalid username or password.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private static string GenerateToken(string sub, string name)
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

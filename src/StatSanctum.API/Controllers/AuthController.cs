using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using StatSanctum.API.Enums;
using StatSanctum.API.Helper;
using StatSanctum.API.Models;
using StatSanctum.API.Queries.Users;
using StatSanctum.Entities;
using System.Security.Claims;

namespace StatSanctum.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IMediator _mediator;
        private IJwtHelper _jwtHelper;

        private const string _cookieName = "AuthToken";
        private const int _hours = 1;

        public AuthController(IMediator mediator, IJwtHelper jwtHelper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _jwtHelper = jwtHelper ?? throw new ArgumentNullException(nameof(jwtHelper));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<string>> Login(UserLoginDto request)
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
                    var token = _jwtHelper.GenerateToken(Convert.ToString(userId), username);

                    // Set the token in a secure HTTP-only cookie
                    Response.Cookies.Append(_cookieName, token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true, // Set to true if using HTTPS
                        SameSite = SameSiteMode.Strict, // Adjust as needed
                        Expires = DateTime.UtcNow.AddHours(_hours)
                    });

                    return Ok(new { message = "Login successful", token});
                }

                return Unauthorized("Invalid username or password.");
            }
            catch (ArgumentException)
            {
                return Unauthorized("Invalid username or password.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("[action]")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return Ok(new { message = "Logged out" });
        }

        [HttpGet("[action]")]
        public IActionResult Secure()
        {
            if (Request.Cookies.TryGetValue(_cookieName, out var token))
            {
                // Validate the token here if needed
                return Ok(new { message = "User is authenticated", token });
            }

            var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if (!string.IsNullOrEmpty(email))
            {
                return Ok(new { message = "User is authenticated", Oauth = $"Hello {email} (Google OAuth)!" });
            }

            return Unauthorized(new { message = "User is not authenticated" });
        }

        [HttpGet("Google")]
        public async Task LoginWithGoogle()
        {
            await HttpContext.ChallengeAsync(
                GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = "/auth/secure"
                });
        }
    }
}

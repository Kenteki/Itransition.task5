using ItransitionTask5.API.Contracts;
using ItransitionTask5.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ItransitionTask5.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            (Guid userId, List<string> errors) = await _authService.RegisterAsync(
                request.Name,
                request.Email,
                request.Position,
                request.Password
            );

            if (errors.Any())
            {
                return BadRequest(new { message = string.Join("; ", errors) });
            }

            return Ok(new
            {
                message = "Registration successful! Please check your email to verify your account.",
                userId = userId
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            (string? token, var user, string? error) = await _authService.LoginAsync(request.Email, request.Password);

            if (error != null)
            {
                return Unauthorized(new { message = error });
            }

            var response = new LoginResponse(
                token!,
                user!.Id,
                user.Name,
                user.Email,
                user.Status
            );

            return Ok(response);
        }

        [HttpGet("verify-email")]
        public async Task<ActionResult> VerifyEmail([FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest(new { message = "Invalid verification token." });
            }

            var success = await _authService.VerifyEmailAsync(token);

            if (!success)
            {
                return BadRequest(new { message = "Email verification failed. Token may be invalid or expired." });
            }

            return Ok(new { message = "Email verified successfully! You can now login." });
        }
    }
}

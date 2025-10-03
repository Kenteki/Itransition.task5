using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ItransitionTask5.Core.Abstractions;

namespace ItransitionTask5.API.Middleware
{
    public class UserStatusMiddleware
    {
        private readonly RequestDelegate _next;

        public UserStatusMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserService userService)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";

            // Skip public endpoints
            if (path.Contains("/api/auth/login") ||
                path.Contains("/api/auth/register") ||
                path.Contains("/api/auth/verify-email") ||
                path.Contains("/swagger"))
            {
                await _next(context);
                return;
            }

            // Check if request is authenticated
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)
                                  ?? context.User.FindFirst(JwtRegisteredClaimNames.Sub);

                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
                {

                    var user = await userService.GetById(userId);

                    if (user == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            message = "User account has been deleted. Please register again."
                        });
                        return;
                    }

                    if (user.Status == "Blocked")
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            message = "Your account has been blocked. Please contact support."
                        });
                        return;
                    }

                }
            }

            // Continue to next middleware/controller
            await _next(context);
        }
    }
}
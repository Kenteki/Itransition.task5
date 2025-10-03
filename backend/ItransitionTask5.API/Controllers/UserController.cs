using ItransitionTask5.API.Contracts;
using ItransitionTask5.Core.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItransitionTask5.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> GetUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();

                var response = users.Select(u => new UserResponse(
                    u.Id,
                    u.Name,
                    u.Email,
                    u.Position,
                    u.RegistrationTime,
                    u.LastLogin,
                    u.Status
                )).ToList();
                return Ok(response);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost("block")]
        public async Task<ActionResult> BlockUsers([FromBody] List<Guid> userIds)
        {
            if (userIds == null || !userIds.Any())
            {
                return BadRequest(new { message = "No users specified for blocking." });
            }

            var success = await _userService.BlockUsers(userIds);

            if (!success)
            {
                return BadRequest(new { message = "Failed to block users." });
            }

            return Ok(new { message = $"Successfully blocked {userIds.Count} user(s)." });
        }

        [HttpPost("unblock")]
        public async Task<ActionResult> UnblockUsers([FromBody] List<Guid> userIds)
        {
            if (userIds == null || !userIds.Any())
            {
                return BadRequest(new { message = "No users specified for unblocking." });
            }

            var success = await _userService.UnblockUsers(userIds);

            if (!success)
            {
                return BadRequest(new { message = "Failed to unblock users." });
            }

            return Ok(new { message = $"Successfully unblocked {userIds.Count} user(s)." });
        }
        [HttpPost("delete")]
        public async Task<ActionResult> DeleteUsers([FromBody] List<Guid> userIds)
        {
            if (userIds == null || !userIds.Any())
            {
                return BadRequest(new { message = "No users specified for deletion." });
            }

            var success = await _userService.DeleteMultiple(userIds);

            if (!success)
            {
                return BadRequest(new { message = "Failed to delete users." });
            }

            return Ok(new { message = $"Successfully deleted {userIds.Count} user(s)." });
        }
        [HttpPost("delete-unverified")]
        public async Task<ActionResult> DeleteUnverified()
        {
            var count = await _userService.DeleteUnverified();

            return Ok(new { message = $"Successfully deleted {count} unverified user(s)." });
        }
    }
}

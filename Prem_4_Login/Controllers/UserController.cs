using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prem_4_Login.API.DTOs;
using Prem_4_Login.API.IServices;
using System.Security.Claims;

namespace Prem_4_Login.API.Controllers
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

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var user =
                await _userService.GetProfileAsync(userId.Value);

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(new
            {
                user.UserId,
                user.LoginId,
                user.ApplicantName,
                user.FatherName,
                user.MobileNumber,
                user.Email,
                user.DOB,
                user.ProfilePic,
                user.RoleId,
                user.RoleName
            });
        }



        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(
            [FromBody] UpdateProfileRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var result = await _userService.UpdateProfileAsync( userId.Value, request,ipAddress );

            if (!result)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(new
            {
                message = "Profile updated successfully."
            });
        }



        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var result = await _userService.ChangePasswordAsync( userId.Value, request, ipAddress);

            if (!result)
            {
                return BadRequest(new
                {
                    message = "Old password is incorrect or user does not exist."
                });
            }

            return Ok(new
            {
                message = "Password changed successfully."
            });
        }

        [HttpGet("admin/users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers([FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 10,[FromQuery] string? search = null)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1 || pageSize > 100)
            {
                pageSize = 10;
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var result = await _userService.GetUsersAsync( pageNumber, pageSize, search, ipAddress);

            return Ok(new
            {
                pageNumber,
                pageSize,
                totalRecords = result.TotalRecords,
                totalPages =
                    (int)Math.Ceiling(
                        result.TotalRecords /
                        (double)pageSize),

                data = result.Users
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ipAddress =
                HttpContext.Connection.RemoteIpAddress?.ToString();

            try
            {
                var userId =
                    await _userService.RegisterAsync(
                        request,
                        ipAddress);

                return Ok(new
                {
                    message = "Registration successful.",
                    userId = userId,
                    loginId = request.MobileNumber,
                    defaultPassword = "12345"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        private int? GetUserId()
        {
            var claim =
                User.FindFirst(
                    ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                claim =
                    User.FindFirst("UserId");
            }

            if (claim == null)
            {
                return null;
            }

            if (int.TryParse(
                    claim.Value,
                    out int userId))
            {
                return userId;
            }

            return null;
        }
    }
}
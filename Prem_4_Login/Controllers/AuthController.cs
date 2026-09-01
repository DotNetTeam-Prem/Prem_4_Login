using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prem_4_Login.API.DTOs;
using Prem_4_Login.API.IServices;

namespace Prem_4_Login.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(
            IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login( LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var result =await _authService.LoginAsync(request,ipAddress);

            if (result == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid User Id or Password."
                });
            }

            return Ok(result);
        }
    }
}
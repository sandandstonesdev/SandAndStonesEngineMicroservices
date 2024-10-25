using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SandAndStones.Api.DTO;
using SandAndStones.Api.Services;
using SandAndStones.Infrastructure.Models;

namespace SandAndStones.Api
{
    [ApiController]
    [Route("[controller]")]
    public class UserAuthorizationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserAuthorizationController(IAuthService authService) : base()
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var success = await _authService.Register(registerRequest);
            if (!success)
                return BadRequest(new { message = "Register Failed." });
            return Ok(new { message = "Register Successful." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var response = await _authService.Login(loginRequest);
            return Ok(new { message = "Login Successful.", data = response });
        }

        [Authorize]
        [HttpGet("logout")]
        public async Task<ActionResult> Logout()
        {
            var success = await _authService.Logout();
            if (!success)
                return BadRequest(new { message = "Logout Failed." });
            return Ok(new { message = "Logout Successful." });
        }

        [Authorize]
        [HttpGet("userInfo/{email}")]
        public async Task<ActionResult> GetUserInfo([FromRoute] string email)
        {
            var userInfo = await _authService.GetUserInfo(email);
            return Ok(new { message = "GetUserInfo Successful.", data = userInfo });
        }
    }
}
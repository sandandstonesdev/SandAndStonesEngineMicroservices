using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SandAndStones.Api.UseCases.User.GetUserInfo;
using SandAndStones.Api.UseCases.User.LoginUser;
using SandAndStones.Api.UseCases.User.LogoutUser;
using SandAndStones.Api.UseCases.User.RegisterUser;

namespace SandAndStones.Api
{
    [ApiController]
    [Route("[controller]")]
    public class UserAuthorizationController(IMediator mediator) : ControllerBase()
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest registerRequest)
        {
            var result = await _mediator.Send(registerRequest);
            if (!result.Success)
                return BadRequest(new { message = "Register Failed." });
            return Ok(new { message = "Register Successful." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest loginRequest)
        {
            var result = await _mediator.Send(loginRequest);
            return Ok(new { message = result.Message, result.AccessToken, result.RefreshToken });
        }

        [Authorize]
        [HttpGet("logout")]
        public async Task<ActionResult> Logout()
        {
            var result = await _mediator.Send(new LogoutUserRequest());

            return result.Success ? Ok(new { message = "Logout Successful." })
                : BadRequest(new { message = "Logout Failed." });
        }

        [Authorize]
        [HttpGet("userInfo/{email}")]
        public async Task<ActionResult> GetUserInfo([FromRoute] string email)
        {
            return Ok(_mediator.Send(new GetUserInfoRequest(email)));
        }
    }
}
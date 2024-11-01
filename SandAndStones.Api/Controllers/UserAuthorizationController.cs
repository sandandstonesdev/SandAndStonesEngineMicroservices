using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SandAndStones.App.UseCases.User.CheckCurrentTokenValidity;
using SandAndStones.App.UseCases.User.GetUserInfo;
using SandAndStones.App.UseCases.User.LoginUser;
using SandAndStones.App.UseCases.User.LogoutUser;
using SandAndStones.App.UseCases.User.RegisterUser;

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

            return Ok(new { message = result.Message, result.Succeeded });
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
            var result = await _mediator.Send(new GetUserInfoRequest(email));

            return Ok(new { message = result.Message, result.UserName, result.Email});;
        }

        [Authorize]
        [HttpGet("currenttokenvalid")]
        public async Task<ActionResult> CheckCurrentTokenValidity()
        {
            var result = await _mediator.Send(new CheckCurrentTokenValidityRequest());

            return Ok(new { result.IsValid });
        }
    }
}
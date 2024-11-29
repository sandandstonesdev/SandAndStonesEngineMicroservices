using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SandAndStones.Gateway.Api.User.CheckCurrentTokenValidity;
using SandAndStones.Gateway.Api.User.GetUserInfo;
using SandAndStones.Gateway.Api.User.LoginUser;
using SandAndStones.Gateway.Api.User.LogoutUser;
using SandAndStones.Gateway.Api.User.RegisterUser;
using SandAndStones.Infrastructure.Models;

namespace SandAndStones.Gateway.Api.Controllers
{
    [ApiController]
    [Route("gateway-api/[controller]")]
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

        [Authorize(Roles = UserRoles.UserRole)]
        [HttpGet("logout")]
        public async Task<ActionResult> Logout()
        {
            var result = await _mediator.Send(new LogoutUserRequest());

            return result.Success ? Ok(new { message = "Logout Successful." })
                : BadRequest(new { message = "Logout Failed." });
        }

        [Authorize(Roles = UserRoles.UserRole)]
        [HttpGet("userInfo/{email}")]
        public async Task<ActionResult> GetUserInfo([FromRoute] string email)
        {
            var result = await _mediator.Send(new GetUserInfoRequest(email));

            return Ok(new { message = result.Message, result.UserName, result.Email }); ;
        }

        [HttpGet("currenttokenvalid")]
        public async Task<ActionResult> CheckCurrentTokenValidity()
        {
            var result = await _mediator.Send(new CheckCurrentTokenValidityRequest());

            return Ok(new { result.IsValid });
        }
    }
}
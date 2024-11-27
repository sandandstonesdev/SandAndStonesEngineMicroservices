using MediatR;
using Microsoft.AspNetCore.Mvc;
using SandAndStones.Gateway.Api.DTO;
using SandAndStones.Gateway.Api.User.Profile;

namespace SandAndStones.Gateway.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserProfileController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpGet("profile")]
        public async Task<ActionResult> GetProfile()
        {
            var result = await _mediator.Send(new GetUserProfileRequest());

            return Ok(new UserProfileDTO(
                result.Email,
                result.UserName,
                result.UserAvatar,
                result.PriviledgeLevel,
                result.CreatedAt,
                result.BirthAt,
                result.CurrentUserIP
            ));
        }
    }
}

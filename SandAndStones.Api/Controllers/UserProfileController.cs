using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SandAndStones.Api.DTO;
using SandAndStones.App.UseCases.Profile;
using SandAndStones.App.UseCases.User.GetUserInfo;

namespace SandAndStones.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserProfileController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [Authorize]
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

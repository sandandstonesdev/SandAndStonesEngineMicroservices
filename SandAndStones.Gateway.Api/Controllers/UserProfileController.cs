using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SandAndStones.Gateway.Api.DTO;
using SandAndStones.Gateway.Api.User.Profile;
using SandAndStones.Infrastructure.Models;

namespace SandAndStones.Gateway.Api.Controllers
{
    [Route("gateway-api/[controller]")]
    [ApiController]
    public class UserProfileController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [Authorize(Roles = UserRoles.UserRole)]
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

using MediatR;
using SandAndStones.Infrastructure.Services;

namespace SandAndStones.Gateway.Api.User.Profile
{
    public class GetUserProfileQuery(
        IUserProfileService userProfileService,
        IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetUserProfileRequest, GetUserProfileResponse>
    {
        private readonly IUserProfileService _userProfileService = userProfileService;
        public async Task<GetUserProfileResponse> Handle(GetUserProfileRequest request, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileService.GetUserProfile(httpContextAccessor.HttpContext);
            return new GetUserProfileResponse(
                userProfile.Email,
                userProfile.UserName,
                userProfile.UserAvatar,
                userProfile.PriviledgeLevel,
                userProfile.CreatedAt,
                userProfile.BirthAt,
                userProfile.CurrentUserIP,
                []);

        }
    }
}

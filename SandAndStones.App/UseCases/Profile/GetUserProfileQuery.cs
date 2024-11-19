using MediatR;
using Microsoft.AspNetCore.Http;
using SandAndStones.Infrastructure.Services;

namespace SandAndStones.App.UseCases.Profile
{
    public class GetUserProfileQuery(
        IUserProfileService userProfileService,
        IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetUserProfileRequest, GetUserProfileResponse>
    {
        private readonly IUserProfileService _userProfileService = userProfileService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
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

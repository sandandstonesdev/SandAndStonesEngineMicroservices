using MediatR;
using Microsoft.AspNetCore.Http;
using SandAndStones.App.Contracts.Services;
using SandAndStones.App.UseCases.Profile.GetUserProfile;

namespace SandAndStones.Infrastructure.Handlers.UseCases.Profile
{
    public class GetUserProfileQueryHandler(
        IUserProfileService userProfileService,
        IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetUserProfileQuery, GetUserProfileQueryResponse>
    {
        private readonly IUserProfileService _userProfileService = userProfileService;
        public async Task<GetUserProfileQueryResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileService.GetUserProfile(httpContextAccessor);
            return new GetUserProfileQueryResponse(
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

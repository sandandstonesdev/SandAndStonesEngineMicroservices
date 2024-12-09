using Microsoft.AspNetCore.Http;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.Contracts.Services;
using SandAndStones.Domain.Entities;

namespace SandAndStones.Infrastructure.Repositories
{
    public class UserProfileRepository(IUserProfileService userProfileService) : IUserProfileRepository
    {
        private readonly IUserProfileService _userProfileService = userProfileService;
        public async Task<UserProfile> GetUserProfile(IHttpContextAccessor contextAccessor)
        {
            return await _userProfileService.GetUserProfile(contextAccessor);
        }
    }
}

using Microsoft.AspNetCore.Http;
using SandAndStones.Domain.Entities;

namespace SandAndStones.App.Contracts.Services
{
    public interface IUserProfileService
    {
        Task<UserProfile> GetUserProfile(IHttpContextAccessor httpContext);
    }
}

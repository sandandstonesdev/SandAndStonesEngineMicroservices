using Microsoft.AspNetCore.Http;
using SandAndStones.Infrastructure.Models;

namespace SandAndStones.Infrastructure.Services
{
    public interface IUserProfileService
    {
        Task<UserProfile> GetUserProfile(HttpContext httpContext);
    }
}

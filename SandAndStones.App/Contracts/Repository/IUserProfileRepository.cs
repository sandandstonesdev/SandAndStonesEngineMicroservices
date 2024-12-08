using Microsoft.AspNetCore.Http;
using SandAndStones.Domain.Entities;

namespace SandAndStones.App.Contracts.Repository
{
    public interface IUserProfileRepository
    {
        Task<UserProfile> GetUserProfile(IHttpContextAccessor contextAccessor);
    }
}

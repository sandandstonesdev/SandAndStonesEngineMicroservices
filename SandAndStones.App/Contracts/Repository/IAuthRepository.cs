using Microsoft.AspNetCore.Http;
using SandAndStones.Domain.DTO;

namespace SandAndStones.App.Contracts.Repository
{
    public interface IAuthRepository
    {
        Task<bool> Register(UserDto userDto);
        Task<TokenDto> Login(UserDto userDto);
        Task<bool> Logout(IHttpContextAccessor httpContext);
        Task<bool> CheckCurrentTokenValidity(IHttpContextAccessor httpContext);
        Task<bool> RefreshUserTokenAsync(IHttpContextAccessor contextAccessor);
        Task<UserInfoDto> GetUserInfo(UserInfoDto userDto, IHttpContextAccessor contextAccessor);
        bool InjectTokensIntoCookie(TokenDto tokenDto, IHttpContextAccessor contextAccessor);
    }
}

using Microsoft.AspNetCore.Http;
using SandAndStones.Domain.DTO;

namespace SandAndStones.Infrastructure.Services
{
    public interface IAuthService
    {
        Task<bool> Register(UserDto userDto);
        Task<TokenDto> Login(UserDto userDto);
        Task<bool> Logout(HttpContext httpContext);
        Task<bool> CheckCurrentTokenValidity(HttpContext httpContext);
        Task<bool> RefreshUserTokenAsync(HttpContext httpContext);
        Task<UserInfoDto> GetUserInfo(UserInfoDto userDto, HttpContext httpContext);
        bool InjectTokensIntoCookie(TokenDto tokenDto, HttpContext context);
    }
}

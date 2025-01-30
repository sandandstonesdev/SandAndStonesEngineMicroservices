using Microsoft.AspNetCore.Http;
using SandAndStones.Domain.DTO;

namespace SandAndStones.Infrastructure.Contracts
{
    public interface IAuthService
    {
        Task<bool> Register(UserDto userDto);
        Task<TokenDto> Login(UserDto userDto);
        Task<bool> Logout(IHttpContextAccessor contextAccessor);
        Task<bool> CheckCurrentTokenValidity(IHttpContextAccessor contextAccessor);
        Task<bool> RefreshUserTokenAsync(IHttpContextAccessor contextAccessor);
        Task<UserInfoDto> GetUserInfo(UserInfoDto userDto, IHttpContextAccessor contextAccessor);
        bool InjectTokensIntoCookie(TokenDto tokenDto, IHttpContextAccessor contextAccessor);
    }
}
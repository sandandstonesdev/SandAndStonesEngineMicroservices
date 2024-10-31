using SandAndStones.Domain.DTO;

namespace SandAndStones.Infrastructure.Services
{
    public interface IAuthService
    {
        Task<bool> Register(UserDto userDto);
        Task<TokenDto> Login(UserDto userDto);
        Task<bool> Logout();
        Task<TokenDto> UserRefreshTokenAsync(TokenDto request);
        Task<UserDto> GetUserInfo(UserInfoDto userDto);
    }
}

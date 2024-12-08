using Microsoft.AspNetCore.Http;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.Domain.DTO;
using SandAndStones.Infrastructure.Contracts;

namespace SandAndStones.Infrastructure.Repositories
{
    public class AuthRepository(IAuthService authService) : IAuthRepository
    {
        private readonly IAuthService _authService = authService;
        public async Task<bool> Register(UserDto userDto)
        {
            return  await _authService.Register(userDto);
        }

        public async Task<TokenDto> Login(UserDto userDto)
        {
            return await _authService.Login(userDto);
        }
        public async Task<bool> Logout(IHttpContextAccessor httpContext)
        {
            return await _authService.Logout(httpContext);
        }
        public async Task<bool> CheckCurrentTokenValidity(IHttpContextAccessor httpContext)
        {
            return await _authService.CheckCurrentTokenValidity(httpContext);
        }
        public async Task<bool> RefreshUserTokenAsync(IHttpContextAccessor contextAccessor)
        {
            return await _authService.RefreshUserTokenAsync(contextAccessor);
        }
        public async Task<UserInfoDto> GetUserInfo(UserInfoDto userDto, IHttpContextAccessor contextAccessor)
        {
            return await _authService.GetUserInfo(userDto, contextAccessor);
        }
        public bool InjectTokensIntoCookie(TokenDto tokenDto, IHttpContextAccessor contextAccessor)
        {
            return _authService.InjectTokensIntoCookie(tokenDto, contextAccessor);
        }
    }
}

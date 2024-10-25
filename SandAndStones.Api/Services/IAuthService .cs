using SandAndStones.Api.DTO;

namespace SandAndStones.Api.Services
{
    public interface IAuthService
    {
        Task<bool> Register(RegisterRequest registerRequest);
        Task<LoginResponse> Login(LoginRequest userLoginRequest);
        Task<bool> Logout();
        Task<RefreshTokenResponse> UserRefreshTokenAsync(RefreshTokenRequest request);
        Task<GetUserInfoResponse> GetUserInfo(string email);
    }
}

using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using SandAndStones.Api.DTO;
using SandAndStones.Domain.DTO;
using SandAndStones.Infrastructure.Data;
using SandAndStones.Infrastructure.Models;

namespace SandAndStones.Infrastructure.Services
{
    public class AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenGenerator tokenGenerator,
        JwtSettings jwtSettings) : IAuthService
    {
        private const string RefreshTokenName = "RefreshToken";
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
        private readonly JwtSettings _jwtSettings = jwtSettings;
        
        public async Task<bool> Register(UserDto userDto)
        {
            try
            {
                ApplicationUser user = new()
                {
                    UserName = userDto.Email,
                    Email = userDto.Email
                };

                var result = await _userManager.CreateAsync(user, userDto.Password);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                throw new Exception($"Register failed: {ex.Message}.");
            }
        }

        public async Task<TokenDto> Login(UserDto userDto)
        {
            try
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(userDto.Email);

                ApplicationUser user = await _userManager.FindByEmailAsync(userDto.Email);
                ArgumentNullException.ThrowIfNull(user, nameof(user));
                
                var result = await _signInManager.CheckPasswordSignInAsync(user, userDto.Password, false);

                if (!result.Succeeded)
                {
                    throw new ArgumentException("Invalid password.");
                }

                await _userManager.RemoveAuthenticationTokenAsync(user, _jwtSettings.RefreshTokenProviderName, RefreshTokenName);
                var refreshToken = await _userManager.GenerateUserTokenAsync(user, _jwtSettings.RefreshTokenProviderName, RefreshTokenName);
                await _userManager.SetAuthenticationTokenAsync(user, _jwtSettings.RefreshTokenProviderName, RefreshTokenName, refreshToken);

                var token = _tokenGenerator.GenerateToken(user.Id, user.Email);
                return new TokenDto(token, refreshToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Login failed: " + ex.Message);
            }
        }

        public async Task<bool> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while logout: " + ex.Message);
            }

            return true;
        }

        public async Task<TokenDto> UserRefreshTokenAsync(TokenDto request)
        {
            try
            {
                var principal = _tokenGenerator.GetPrincipalFromExpiredToken(request.AccessToken);
                ArgumentNullException.ThrowIfNull(principal, nameof(principal));
                ArgumentNullException.ThrowIfNull(principal.FindFirst("UserName")?.Value, "UserName.Value");

                var user = await _userManager.FindByNameAsync(principal.FindFirst("UserName")?.Value ?? "");
                ArgumentNullException.ThrowIfNull(user, nameof(user));
                ArgumentNullException.ThrowIfNullOrWhiteSpace(user.Email, nameof(user.Email));

                var result = await _userManager.VerifyUserTokenAsync(user, _jwtSettings.RefreshTokenProviderName, RefreshTokenName, request.RefreshToken);

                if (!result) throw new Exception("Refresh token is not valid.");
                
                var accessToken = _tokenGenerator.GenerateToken(user.Id, user.Email);

                return new TokenDto(accessToken, request.RefreshToken);
            }
            catch(Exception ex)
            {
                throw new Exception("Error while refreshing token: " + ex.Message);
            }
        }

        public async Task<UserDto> GetUserInfo(UserInfoDto userDto)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userDto.Email, nameof(userDto.Email));

            ApplicationUser userInfo = await _userManager.FindByEmailAsync(userDto.Email);
            ArgumentNullException.ThrowIfNull(userInfo, nameof(userInfo));
            ArgumentException.ThrowIfNullOrWhiteSpace(userInfo.UserName, nameof(userInfo.UserName));
            ArgumentException.ThrowIfNullOrWhiteSpace(userInfo.Email, nameof(userInfo.Email));

            return new UserDto(userInfo.UserName, userInfo.Email);
        }
    }
}

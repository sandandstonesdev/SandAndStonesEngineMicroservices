using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SandAndStones.Domain.Constants;
using SandAndStones.Domain.DTO;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Models;
using System.Security.Claims;

namespace SandAndStones.Infrastructure.Services
{
    public class AuthService(
        UserManager<ApplicationUser> userManager,
        ITokenGenerator tokenGenerator
    ) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
        
        public async Task<bool> Register(UserDto userDto)
        {
            try
            {
                ApplicationUser user = new()
                {
                    UserName = userDto.Email,
                    Email = userDto.Email,
                };

                var result = await _userManager.CreateAsync(user, userDto.Password);
                if (!result.Succeeded)
                {
                    throw new Exception($"Cannot create user: {userDto.Email} ");
                }

                await userManager.AddToRoleAsync(user, UserRoles.UserRole);
                await userManager.AddToRoleAsync(user, UserRoles.AdminRole);

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
                ArgumentException.ThrowIfNullOrWhiteSpace(userDto.Email, nameof(userDto.Email));

                ApplicationUser user = await _userManager.FindByEmailAsync(userDto.Email);
                
                ArgumentNullException.ThrowIfNull(user, nameof(user));

                var result = await _userManager.CheckPasswordAsync(user, userDto.Password);

                if (!result)
                {
                    throw new ArgumentException("Invalid password.");
                }

                var userRoles = await userManager.GetRolesAsync(user);

                var token = _tokenGenerator.GenerateToken(user.Id, userRoles, user.Email!);
                var refreshToken = _tokenGenerator.GenerateToken(user.Id, userRoles, user.Email!);

                return new TokenDto(token, refreshToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Login failed: " + ex.Message);
            }
        }

        public async Task<bool> Logout(IHttpContextAccessor contextAccessor)
        {
            try
            {
                contextAccessor.HttpContext.Response.Cookies.Delete(
                    JwtTokenConstants.AccessTokenName,
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(-1),
                        HttpOnly = true,
                        IsEssential = true,
                        Secure = true,
                        SameSite = SameSiteMode.None
                    });
                contextAccessor.HttpContext.Response.Cookies.Delete(JwtTokenConstants.RefreshTokenName,
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(-1),
                        HttpOnly = true,
                        IsEssential = true,
                        Secure = true,
                        SameSite = SameSiteMode.None
                    });
            }
            catch (Exception ex)
            {
                throw new Exception("Error while logout: " + ex.Message);
            }

            return true;
        }

        public async Task<bool> CheckCurrentTokenValidity(IHttpContextAccessor contextAccessor)
        {
            contextAccessor.HttpContext.Request.Cookies.TryGetValue(JwtTokenConstants.AccessTokenName, out var accessToken);
            var user = await GetUserByToken(accessToken);
            return user != null;
        }

        internal async Task<ApplicationUser> GetUserByToken(string token)
        {
            var principal = _tokenGenerator.GetPrincipalFromToken(token);
            ArgumentNullException.ThrowIfNull(principal, nameof(principal));

            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            ArgumentException.ThrowIfNullOrWhiteSpace(email, "email.Value");

            var user = await _userManager.FindByEmailAsync(email);
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentException.ThrowIfNullOrWhiteSpace(user.Email, nameof(user.Email));

            return user;
        }

        public async Task<bool> RefreshUserTokenAsync(IHttpContextAccessor contextAccessor)
        {
            try
            {
                contextAccessor.HttpContext.Request.Cookies.TryGetValue(JwtTokenConstants.AccessTokenName, out var accessToken);
                contextAccessor.HttpContext.Request.Cookies.TryGetValue(JwtTokenConstants.RefreshTokenName, out var refreshToken);

                var user = await GetUserByToken(accessToken);
                ArgumentNullException.ThrowIfNull(user, nameof(user));
                ArgumentException.ThrowIfNullOrWhiteSpace(user.Email, nameof(user.Email));

                var userRoles = await userManager.GetRolesAsync(user);

                var newAccessToken = _tokenGenerator.GenerateToken(user.Id, userRoles, user.Email);
                var tokenDto = new TokenDto(accessToken, refreshToken);
                
                InjectTokensIntoCookie(tokenDto, contextAccessor);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while refreshing token: " + ex.Message);
            }
        }

        public async Task<UserInfoDto> GetUserInfo(UserInfoDto userDto, IHttpContextAccessor contextAccessor)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userDto.Email, nameof(userDto.Email));

            var userInfo = await _userManager.FindByEmailAsync(userDto.Email);
            ArgumentNullException.ThrowIfNull(userInfo, nameof(userInfo));
            ArgumentException.ThrowIfNullOrWhiteSpace(userInfo.UserName, nameof(userInfo.UserName));
            ArgumentException.ThrowIfNullOrWhiteSpace(userInfo.Email, nameof(userInfo.Email));

            contextAccessor.HttpContext.Request.Cookies.TryGetValue(JwtTokenConstants.AccessTokenName, out var accessToken);
            
            var user = await GetUserByToken(accessToken);
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentException.ThrowIfNullOrWhiteSpace(user.Email, nameof(user.Email));

            return new UserInfoDto(userInfo.UserName, userInfo.Email, true);
        }

        public bool InjectTokensIntoCookie(TokenDto tokenDto, IHttpContextAccessor contextAccessor)
        {
            try
            {
                contextAccessor.HttpContext.Response.Cookies.Append(JwtTokenConstants.AccessTokenName, tokenDto.AccessToken,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMinutes(15),
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
                contextAccessor.HttpContext.Response.Cookies.Append(JwtTokenConstants.RefreshTokenName, tokenDto.RefreshToken,
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(15),
                        HttpOnly = true,
                        IsEssential = true,
                        Secure = true,
                        SameSite = SameSiteMode.None
                    });
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
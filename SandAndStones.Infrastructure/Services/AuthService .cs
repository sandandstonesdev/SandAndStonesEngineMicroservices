﻿using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SandAndStones.Api.DTO;
using SandAndStones.Domain.Constants;
using SandAndStones.Domain.DTO;
using SandAndStones.Infrastructure.Models;
using System.Net.Http;
using System.Security.Claims;

namespace SandAndStones.Infrastructure.Services
{
    public class AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenGenerator tokenGenerator,
        JwtSettings jwtSettings) : IAuthService
    {
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

                var token = _tokenGenerator.GenerateToken(user.Id, user.Email);
                var refreshToken = _tokenGenerator.GenerateToken(user.Id, user.Email);

                return new TokenDto(token, refreshToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Login failed: " + ex.Message);
            }
        }

        public async Task<bool> Logout(HttpContext httpContext)
        {
            try
            {
                await _signInManager.SignOutAsync();

                httpContext.Response.Cookies.Delete(
                    JwtTokenConstants.AccessTokenName,
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(-1),
                        HttpOnly = true,
                        IsEssential = true,
                        Secure = true,
                        SameSite = SameSiteMode.None
                    });
                httpContext.Response.Cookies.Delete(JwtTokenConstants.RefreshTokenName,
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

        public async Task<bool> CheckCurrentTokenValidity(HttpContext httpContext)
        {
            httpContext.Request.Cookies.TryGetValue(JwtTokenConstants.AccessTokenName, out var accessToken);
            var user = await GetUserByToken(accessToken);
            return user != null;
        }

        private async Task<ApplicationUser> GetUserByToken(string token)
        {
            var principal = _tokenGenerator.GetPrincipalFromExpiredToken(token);
            ArgumentNullException.ThrowIfNull(principal, nameof(principal));

            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            ArgumentException.ThrowIfNullOrWhiteSpace(email, "email.Value");

            var user = await _userManager.FindByEmailAsync(email);
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentException.ThrowIfNullOrWhiteSpace(user.Email, nameof(user.Email));

            return user;
        }

        public async Task<bool> RefreshUserTokenAsync(HttpContext httpContext)
        {
            try
            {
                httpContext.Request.Cookies.TryGetValue(JwtTokenConstants.AccessTokenName, out var accessToken);
                httpContext.Request.Cookies.TryGetValue(JwtTokenConstants.RefreshTokenName, out var refreshToken);

                var user = await GetUserByToken(accessToken);
                ArgumentNullException.ThrowIfNull(user, nameof(user));
                ArgumentException.ThrowIfNullOrWhiteSpace(user.Email, nameof(user.Email));

                var newAccessToken = _tokenGenerator.GenerateToken(user.Id, user.Email);
                var tokenDto = new TokenDto(accessToken, refreshToken);
                
                InjectTokensIntoCookie(tokenDto, httpContext);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while refreshing token: " + ex.Message);
            }
        }

        public async Task<UserInfoDto> GetUserInfo(UserInfoDto userDto, HttpContext httpContext)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userDto.Email, nameof(userDto.Email));

            var userInfo = await _userManager.FindByEmailAsync(userDto.Email);
            ArgumentNullException.ThrowIfNull(userInfo, nameof(userInfo));
            ArgumentException.ThrowIfNullOrWhiteSpace(userInfo.UserName, nameof(userInfo.UserName));
            ArgumentException.ThrowIfNullOrWhiteSpace(userInfo.Email, nameof(userInfo.Email));

            httpContext.Request.Cookies.TryGetValue(JwtTokenConstants.AccessTokenName, out var accessToken);
            httpContext.Request.Cookies.TryGetValue(JwtTokenConstants.RefreshTokenName, out var refreshToken);

            var user = await GetUserByToken(accessToken);
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentException.ThrowIfNullOrWhiteSpace(user.Email, nameof(user.Email));

            return new UserInfoDto(userInfo.UserName, userInfo.Email, true);
        }

        public bool InjectTokensIntoCookie(TokenDto tokenDto, HttpContext context)
        {
            try
            {
                context.Response.Cookies.Append(JwtTokenConstants.AccessTokenName, tokenDto.AccessToken,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMinutes(15),
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
                context.Response.Cookies.Append(JwtTokenConstants.RefreshTokenName, tokenDto.RefreshToken,
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
using Microsoft.AspNetCore.Identity;
using SandAndStones.Api.DTO;
using SandAndStones.Api.Exceptions;
using SandAndStones.Infrastructure.Data;
using SandAndStones.Infrastructure.Models;

namespace SandAndStones.Api.Services
{
    public class AuthService : IAuthService
    {
        private const string RefreshTokenName = "RefreshToken";
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ApplicationDbContext _context;
        private readonly JwtSettings _jwtSettings;
        
        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ITokenGenerator tokenGenerator,
            ApplicationDbContext context,
            JwtSettings jwtSettings
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenGenerator = tokenGenerator;
            _context = context;
            _jwtSettings = jwtSettings;
        }

        public async Task<bool> Register(RegisterRequest registerRequest)
        {
            try
            {
                ApplicationUser user = new()
                {
                    UserName = registerRequest.Email,
                    Email = registerRequest.Email
                };

                var result = await _userManager.CreateAsync(user, registerRequest.Password);

                if (!result.Succeeded)
                {
                    throw new RegisterException("Register failed: Cannot create user.");
                }
            }
            catch (Exception ex)
            {
                throw new RegisterException($"Register failed: {ex.Message}.");
            }

            return true;
        }

        public async Task<LoginResponse> Login(LoginRequest userLoginRequest)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(userLoginRequest.Email) ?? throw new LoginException("Login failed: User Email doesn't exist");
                if (user is null || string.IsNullOrWhiteSpace(user.Email))
                {
                    throw new LoginException("Login failed: User Email doesn't exist");
                }


                var result = await _signInManager.CheckPasswordSignInAsync(user, userLoginRequest.Password, false);

                if (!result.Succeeded)
                {
                    throw new LoginException("Login failed: Check your login credentials and try again");
                }

                await _userManager.RemoveAuthenticationTokenAsync(user, _jwtSettings.RefreshTokenProviderName, RefreshTokenName);
                var refreshToken = await _userManager.GenerateUserTokenAsync(user, _jwtSettings.RefreshTokenProviderName, RefreshTokenName);
                await _userManager.SetAuthenticationTokenAsync(user, _jwtSettings.RefreshTokenProviderName, RefreshTokenName, refreshToken);

                var token = _tokenGenerator.GenerateToken(user.Id, user.Email);
                return new LoginResponse(token, refreshToken);
            }
            catch (Exception ex)
            {
                throw new LoginException("Login failed: " + ex.Message);
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
                throw new LogoutException("Error while logout: " + ex.Message);
            }

            return true;
        }

        public async Task<RefreshTokenResponse> UserRefreshTokenAsync(RefreshTokenRequest request)
        {
            var principal = _tokenGenerator.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null || principal.FindFirst("UserName")?.Value == null)
            {
                throw new RefreshTokenException("RefreshToken: User not found by email.");
            }
            else
            {
                var user = await _userManager.FindByNameAsync(principal.FindFirst("UserName")?.Value ?? "");
                if (user == null || string.IsNullOrWhiteSpace(user.Email))
                {
                    throw new RefreshTokenException("RefreshToken: User not found by email");
                }
                else
                {
                    if (!await _userManager.VerifyUserTokenAsync(user, _jwtSettings.RefreshTokenProviderName, RefreshTokenName, request.RefreshToken))
                    {
                        throw new RefreshTokenException("RefreshToken: Refresh token expired");
                    }
                    var accessToken = _tokenGenerator.GenerateToken(user.Id, user.Email);
                    return new RefreshTokenResponse(accessToken, request.RefreshToken);
                }
            }
        }

        public async Task<GetUserInfoResponse> GetUserInfo(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new GetUserInfoException("GetUserInfo: Email is not set");
            }

            ApplicationUser userInfo = await _userManager.FindByEmailAsync(email) ?? throw new GetUserInfoException("GetUserInfo: Cannot find user by email."); ;
            if (userInfo is null || string.IsNullOrWhiteSpace(userInfo.UserName) || string.IsNullOrWhiteSpace(userInfo.Email))
            {
                throw new GetUserInfoException("GetUserInfo: Something went wrong, please try again.");
            }

            return new GetUserInfoResponse(userInfo.UserName, userInfo.Email);
        }
    }
}

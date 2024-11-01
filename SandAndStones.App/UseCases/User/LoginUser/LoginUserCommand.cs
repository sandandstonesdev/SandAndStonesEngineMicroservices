using MediatR;
using Microsoft.AspNetCore.Http;
using SandAndStones.Domain.DTO;
using SandAndStones.Infrastructure.Services;

namespace SandAndStones.App.UseCases.User.LoginUser
{
    public class LoginUserCommand(IHttpContextAccessor httpContextAccessor, IAuthService authService) : IRequestHandler<LoginUserRequest, LoginUserResponse>
    {
        private readonly IAuthService _authService = authService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public async Task<LoginUserResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(request.Email);
            ArgumentException.ThrowIfNullOrWhiteSpace(request.Password);
            
            var tokenDto = await _authService.Login(new UserDto(request.Email, request.Password));
            _authService.InjectTokensIntoCookie(tokenDto, _httpContextAccessor.HttpContext);

            return new LoginUserResponse(true, "Login Successful.");
        }
    }
}

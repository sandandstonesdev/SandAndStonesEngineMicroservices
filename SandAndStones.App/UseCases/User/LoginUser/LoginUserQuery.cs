using MediatR;
using SandAndStones.Domain.DTO;
using SandAndStones.Infrastructure.Services;

namespace SandAndStones.Api.UseCases.User.LoginUser
{
    public class LoginUserQuery(IAuthService authService) : IRequestHandler<LoginUserRequest, LoginUserResponse>
    {
        private readonly IAuthService _authService = authService;
        
        public async Task<LoginUserResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(request.Email);
            ArgumentException.ThrowIfNullOrWhiteSpace(request.Password);
            //
            var tokenDto = await _authService.Login(new UserDto(request.Email, request.Password));
            
            return new LoginUserResponse(tokenDto.AccessToken, tokenDto.RefreshToken, "Login Successful.");
        }
    }
}

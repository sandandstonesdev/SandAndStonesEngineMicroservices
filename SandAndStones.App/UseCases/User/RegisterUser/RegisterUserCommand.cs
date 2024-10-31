using MediatR;
using SandAndStones.Infrastructure.Services;

namespace SandAndStones.Api.UseCases.User.RegisterUser
{
    public class RegisterUserCommand(IAuthService authService) : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
    {
        private readonly IAuthService _authService = authService;
        
        public async Task<RegisterUserResponse> Handle(RegisterUserRequest registerUserRequest, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(registerUserRequest.Email);
            ArgumentException.ThrowIfNullOrWhiteSpace(registerUserRequest.Password);

            bool result = await _authService.Register(new Domain.DTO.UserDto(registerUserRequest.Email, registerUserRequest.Password));
            return new RegisterUserResponse(result);
        }
    }
}

using MediatR;
using SandAndStones.App.UseCases.User.RegisterUser;
using SandAndStones.Infrastructure.Contracts;

namespace SandAndStones.Infrastructure.Handlers.UseCases.User
{
    public class RegisterUserCommandHandler(IAuthService authService) : IRequestHandler<RegisterUserCommand, RegisterUserCommandResponse>
    {
        private readonly IAuthService _authService = authService;

        public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommand registerUserRequest, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(registerUserRequest.Email);
            ArgumentException.ThrowIfNullOrWhiteSpace(registerUserRequest.Password);

            bool result = await _authService.Register(new Domain.DTO.UserDto(registerUserRequest.Email, registerUserRequest.Password));
            return new(result);
        }
    }
}

using MediatR;
using SandAndStones.Infrastructure.Services;

namespace SandAndStones.Api.UseCases.User.LogoutUser
{
    public class LogoutUserCommand : IRequestHandler<LogoutUserRequest, LogoutUserResponse>
    {
        private readonly IAuthService _authService;
        public LogoutUserCommand(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LogoutUserResponse> Handle(LogoutUserRequest request, CancellationToken cancellationToken)
        {
            bool res = await _authService.Logout();

            return new LogoutUserResponse(res);
        }
    }
}

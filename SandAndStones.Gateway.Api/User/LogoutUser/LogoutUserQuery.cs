using MediatR;
using SandAndStones.Infrastructure.Services;

namespace SandAndStones.Gateway.Api.User.LogoutUser
{
    public class LogoutUserQuery(
        IHttpContextAccessor httpContextAccessor,
        IAuthService authService) : IRequestHandler<LogoutUserRequest, LogoutUserResponse>
    {
        private readonly IAuthService _authService = authService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<LogoutUserResponse> Handle(LogoutUserRequest request, CancellationToken cancellationToken)
        {
            bool res = await _authService.Logout(_httpContextAccessor.HttpContext);

            return new(res);
        }
    }
}

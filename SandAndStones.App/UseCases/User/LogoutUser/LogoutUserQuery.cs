using MediatR;
using Microsoft.AspNetCore.Http;
using SandAndStones.Infrastructure.Services;

namespace SandAndStones.App.UseCases.User.LogoutUser
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

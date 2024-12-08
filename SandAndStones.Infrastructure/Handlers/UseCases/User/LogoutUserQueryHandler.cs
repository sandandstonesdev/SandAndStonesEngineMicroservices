using MediatR;
using Microsoft.AspNetCore.Http;
using SandAndStones.App.UseCases.User.LogoutUser;
using SandAndStones.Infrastructure.Contracts;

namespace SandAndStones.Infrastructure.Handlers.UseCases.User
{
    public class LogoutUserQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IAuthService authService) : IRequestHandler<LogoutUserQuery, LogoutUserQueryResponse>
    {
        private readonly IAuthService _authService = authService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<LogoutUserQueryResponse> Handle(LogoutUserQuery request, CancellationToken cancellationToken)
        {
            bool res = await _authService.Logout(_httpContextAccessor);

            return new(res);
        }
    }
}

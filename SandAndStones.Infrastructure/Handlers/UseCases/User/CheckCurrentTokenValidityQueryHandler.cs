using MediatR;
using Microsoft.AspNetCore.Http;
using SandAndStones.App.UseCases.User.CheckCurrentTokenValidity;
using SandAndStones.Infrastructure.Contracts;

namespace SandAndStones.Infrastructure.Handlers.UseCases.User
{
    public class CheckCurrentTokenValidityQueryHandler(IAuthService authService, IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<CheckCurrentTokenValidityQuery, CheckCurrentTokenValidityQueryResponse>
    {
        private readonly IAuthService _authService = authService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<CheckCurrentTokenValidityQueryResponse> Handle(CheckCurrentTokenValidityQuery request, CancellationToken cancellationToken)
        {
            var result = await _authService.CheckCurrentTokenValidity(_httpContextAccessor);
            return new(result);
        }
    }
}

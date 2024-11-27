using MediatR;
using SandAndStones.Infrastructure.Services;

namespace SandAndStones.Gateway.Api.User.CheckCurrentTokenValidity
{
    public class CheckCurrentTokenValidityQuery(IAuthService authService, IHttpContextAccessor httpContextAccessor) : IRequestHandler<CheckCurrentTokenValidityRequest, CheckCurrentTokenValidityResponse>
    {
        private readonly IAuthService _authService = authService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<CheckCurrentTokenValidityResponse> Handle(CheckCurrentTokenValidityRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.CheckCurrentTokenValidity(_httpContextAccessor.HttpContext!);
            return new(result);
        }
    }
}

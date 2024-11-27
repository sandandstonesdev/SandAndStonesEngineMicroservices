using MediatR;
using SandAndStones.Domain.DTO;
using SandAndStones.Infrastructure.Services;

namespace SandAndStones.Gateway.Api.User.GetUserInfo
{
    public class GetUserInfoQuery(IAuthService authService, IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetUserInfoRequest, GetUserInfoResponse>
    {
        private readonly IAuthService _authService = authService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<GetUserInfoResponse> Handle(GetUserInfoRequest request, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(request.UserEmail, nameof(request.UserEmail));

            var userInfo = await _authService.GetUserInfo(new UserInfoDto(request.UserEmail, request.UserEmail), _httpContextAccessor.HttpContext!);
            return new(userInfo.Email, userInfo.Email, "GetUserInfo Successful.");
        }
    }
}

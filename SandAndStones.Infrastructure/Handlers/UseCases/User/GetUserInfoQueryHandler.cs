using MediatR;
using Microsoft.AspNetCore.Http;
using SandAndStones.App.UseCases.User.GetUserInfo;
using SandAndStones.Domain.DTO;
using SandAndStones.Infrastructure.Contracts;

namespace SandAndStones.Infrastructure.Handlers.UseCases.User
{
    public class GetUserInfoQueryHandler(IAuthService authService, IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetUserInfoQuery, GetUserInfoQueryResponse>
    {
        private readonly IAuthService _authService = authService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<GetUserInfoQueryResponse> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(request.UserEmail, nameof(request.UserEmail));

            var userInfo = await _authService.GetUserInfo(new UserInfoDto(request.UserEmail, request.UserEmail), _httpContextAccessor);
            return new(userInfo.Email, userInfo.Email, "GetUserInfo Successful.");
        }
    }
}

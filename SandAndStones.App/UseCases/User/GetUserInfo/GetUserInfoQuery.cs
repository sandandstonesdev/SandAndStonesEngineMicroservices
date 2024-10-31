using MediatR;
using SandAndStones.Domain.DTO;
using SandAndStones.Infrastructure.Services;

namespace SandAndStones.Api.UseCases.User.GetUserInfo
{

    public class GetUserInfoQuery(IAuthService authService) : IRequestHandler<GetUserInfoRequest, GetUserInfoResponse>
    {
        private readonly IAuthService _authService = authService;
        
        public async Task<GetUserInfoResponse> Handle(GetUserInfoRequest request, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(request.UserEmail, nameof(request.UserEmail));

            var userInfo = await _authService.GetUserInfo(new UserInfoDto(request.UserEmail));
            return new GetUserInfoResponse(userInfo.Email, userInfo.Email, "GetUserInfo Successful.");
        }
    }
}

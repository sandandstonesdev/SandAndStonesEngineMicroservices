using MediatR;

namespace SandAndStones.Gateway.Api.User.GetUserInfo
{
    public record GetUserInfoRequest(string UserEmail) : IRequest<GetUserInfoResponse>;
}

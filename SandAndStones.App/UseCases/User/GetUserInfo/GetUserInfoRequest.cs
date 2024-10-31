using MediatR;

namespace SandAndStones.Api.UseCases.User.GetUserInfo
{
    public record GetUserInfoRequest(string UserEmail) : IRequest<GetUserInfoResponse>;
}

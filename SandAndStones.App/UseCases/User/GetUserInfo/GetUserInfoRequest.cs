using MediatR;

namespace SandAndStones.App.UseCases.User.GetUserInfo
{
    public record GetUserInfoRequest(string UserEmail) : IRequest<GetUserInfoResponse>;
}

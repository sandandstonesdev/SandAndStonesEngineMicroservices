using MediatR;

namespace SandAndStones.App.UseCases.User.GetUserInfo
{
    public record GetUserInfoQuery(string UserEmail) : IRequest<GetUserInfoQueryResponse>;
}

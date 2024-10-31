using MediatR;

namespace SandAndStones.Api.UseCases.User.GetUserInfo
{
    public record GetUserInfoResponse
    (
        string UserName,
        string Email,
        string Message
    ) : IRequest;
}

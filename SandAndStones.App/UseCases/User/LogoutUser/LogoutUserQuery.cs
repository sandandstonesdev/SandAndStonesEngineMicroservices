using MediatR;

namespace SandAndStones.App.UseCases.User.LogoutUser
{
    public record LogoutUserQuery() : IRequest<LogoutUserQueryResponse>;
}

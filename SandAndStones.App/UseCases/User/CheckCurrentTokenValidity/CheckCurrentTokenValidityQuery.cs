using MediatR;

namespace SandAndStones.App.UseCases.User.CheckCurrentTokenValidity
{
    public record CheckCurrentTokenValidityQuery() : IRequest<CheckCurrentTokenValidityQueryResponse>;
}

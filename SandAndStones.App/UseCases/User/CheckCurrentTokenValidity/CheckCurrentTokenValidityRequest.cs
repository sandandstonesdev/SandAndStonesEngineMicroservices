using MediatR;

namespace SandAndStones.App.UseCases.User.CheckCurrentTokenValidity
{
    public record CheckCurrentTokenValidityRequest() : IRequest<CheckCurrentTokenValidityResponse>;
}

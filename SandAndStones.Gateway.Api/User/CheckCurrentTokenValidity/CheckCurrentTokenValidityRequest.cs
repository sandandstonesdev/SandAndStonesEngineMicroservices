using MediatR;

namespace SandAndStones.Gateway.Api.User.CheckCurrentTokenValidity
{
    public record CheckCurrentTokenValidityRequest() : IRequest<CheckCurrentTokenValidityResponse>;
}

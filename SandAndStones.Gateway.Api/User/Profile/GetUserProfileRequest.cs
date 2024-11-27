using MediatR;

namespace SandAndStones.Gateway.Api.User.Profile
{
    public record GetUserProfileRequest() : IRequest<GetUserProfileResponse>;
}

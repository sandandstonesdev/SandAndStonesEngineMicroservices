using MediatR;

namespace SandAndStones.App.UseCases.Profile
{
    public record GetUserProfileRequest() : IRequest<GetUserProfileResponse>;
}

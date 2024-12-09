using MediatR;

namespace SandAndStones.App.UseCases.Profile.GetUserProfile
{
    public record GetUserProfileQuery() : IRequest<GetUserProfileQueryResponse>;
}

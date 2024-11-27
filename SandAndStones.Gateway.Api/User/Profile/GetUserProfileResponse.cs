namespace SandAndStones.Gateway.Api.User.Profile
{
    public record GetUserProfileResponse(
        string Email,
        string UserName,
        string UserAvatar,
        string PriviledgeLevel,
        DateTime CreatedAt,
        DateTime BirthAt,
        string CurrentUserIP,
        List<string> LastEvents
    );
}

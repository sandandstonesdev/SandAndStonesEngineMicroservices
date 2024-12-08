namespace SandAndStones.App.UseCases.Profile.GetUserProfile
{
    public record GetUserProfileQueryResponse(
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

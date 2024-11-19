namespace SandAndStones.Infrastructure.Models
{
    public record UserProfile(
        string Email,
        string UserName,
        string PhoneNumber,
        string UserAvatar,
        string PriviledgeLevel,
        DateTime CreatedAt,
        DateTime BirthAt,
        string CurrentUserIP,
        List<string> LastEvents
    );
}

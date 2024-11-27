namespace SandAndStones.Gateway.Api.DTO
{
    public record UserProfileDTO
    (
        string Email,
        string UserName,
        string UserAvatar,
        string PriviledgeLevel,
        DateTime CreatedAt,
        DateTime BirthAt,
        string CurrentUserIP
    );
}

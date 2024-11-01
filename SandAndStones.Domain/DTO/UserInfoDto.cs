namespace SandAndStones.Domain.DTO
{
    public record UserInfoDto(string UserName, string Email, bool IsLogged=false);
}

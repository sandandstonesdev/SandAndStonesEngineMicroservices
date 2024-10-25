namespace SandAndStones.Api.DTO
{
    public record LoginResponse
    (
        string AccessToken,
        string RefreshToken
    );
}

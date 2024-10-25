namespace SandAndStones.Api.DTO
{
    public record RefreshTokenResponse
    (
        string AccessToken,
        string RefreshToken
    );
}

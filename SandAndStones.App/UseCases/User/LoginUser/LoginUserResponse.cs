namespace SandAndStones.Api.UseCases.User.LoginUser
{
    public record LoginUserResponse
    (
        string AccessToken,
        string RefreshToken,
        string Message
    );
}

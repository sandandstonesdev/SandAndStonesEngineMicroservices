namespace SandAndStones.App.UseCases.User.LoginUser
{
    public record LoginUserQueryResponse
    (
        bool Succeeded,
        string Message
    );
}

using MediatR;

namespace SandAndStones.App.UseCases.User.RegisterUser
{
    public record RegisterUserRequest(
        string Username,
        string Email,
        string Password,
        string ConfirmedPassword
    ) : IRequest<RegisterUserResponse>;
}

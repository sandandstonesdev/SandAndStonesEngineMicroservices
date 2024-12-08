using MediatR;

namespace SandAndStones.App.UseCases.User.RegisterUser
{
    public record RegisterUserCommand(
        string Username,
        string Email,
        string Password,
        string ConfirmedPassword
    ) : IRequest<RegisterUserCommandResponse>;
}

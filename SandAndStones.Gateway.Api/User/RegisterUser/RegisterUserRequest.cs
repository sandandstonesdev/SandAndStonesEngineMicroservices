using MediatR;

namespace SandAndStones.Gateway.Api.User.RegisterUser
{
    public record RegisterUserRequest(
        string Username,
        string Email,
        string Password,
        string ConfirmedPassword
    ) : IRequest<RegisterUserResponse>;
}

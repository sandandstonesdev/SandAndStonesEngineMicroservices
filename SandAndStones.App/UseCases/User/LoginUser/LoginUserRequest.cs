using MediatR;

namespace SandAndStones.Api.UseCases.User.LoginUser
{
    public class LoginUserRequest : IRequest<LoginUserResponse>
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public bool Remember { get; set; } = false;
    }
}

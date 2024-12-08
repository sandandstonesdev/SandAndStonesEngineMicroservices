using MediatR;

namespace SandAndStones.App.UseCases.User.LoginUser
{
    public class LoginUserQuery : IRequest<LoginUserQueryResponse>
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public bool Remember { get; set; } = false;
    }
}

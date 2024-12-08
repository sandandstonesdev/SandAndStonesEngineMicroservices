using MediatR;
using Microsoft.AspNetCore.Http;
using SandAndStones.App.UseCases.User.LoginUser;
using SandAndStones.Domain.DTO;
using SandAndStones.Infrastructure.Contracts;

namespace SandAndStones.Infrastructure.Handlers.UseCases.User
{
    public class LoginUserQueryHandler(IHttpContextAccessor httpContextAccessor, IAuthService authService) : IRequestHandler<LoginUserQuery, LoginUserQueryResponse>
    {
        private readonly IAuthService _authService = authService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public async Task<LoginUserQueryResponse> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(request.Email);
            ArgumentException.ThrowIfNullOrWhiteSpace(request.Password);

            var tokenDto = await _authService.Login(new UserDto(request.Email, request.Password));
            _authService.InjectTokensIntoCookie(tokenDto, _httpContextAccessor);

            return new(true, "Login Successful.");
        }
    }
}

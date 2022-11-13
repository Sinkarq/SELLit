using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SELLit.Common;

namespace SELLit.Server.Features.Identity.Commands.Login;

public sealed class LoginCommand : IRequest<OneOf<LoginCommandResponseModel, InvalidLoginCredentials>>
{
    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public sealed class LoginCommandRequestHandler : 
        IRequestHandler<LoginCommand, OneOf<LoginCommandResponseModel, InvalidLoginCredentials>>
    {
        private readonly UserManager<User> userManager;
        private readonly IIdentityService identityService;
        private readonly AppSettings appSettings;

        public LoginCommandRequestHandler(UserManager<User> userManager, IIdentityService identityService,
            IOptions<AppSettings> appSettings)
        {
            this.userManager = userManager;
            this.identityService = identityService;
            this.appSettings = appSettings.Value;
        }

        public async ValueTask<OneOf<LoginCommandResponseModel, InvalidLoginCredentials>> Handle(LoginCommand request,
            CancellationToken cancellationToken)
        {
            var user = await this.userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return new InvalidLoginCredentials();
            }

            var passwordValid = await this.userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordValid)
            {
                return new InvalidLoginCredentials();
            }

            var roles = await this.userManager.GetRolesAsync(user);

            var encryptedToken = this.identityService.GenerateJwtToken(user.Id, user.UserName!, appSettings.Secret, roles);

            return new LoginCommandResponseModel()
            {
                Token = encryptedToken
            };
        }
    }
}
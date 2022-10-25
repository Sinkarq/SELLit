using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OneOf;
using OneOf.Types;
using SELLit.Common;
using SELLit.Server.Infrastructure;

namespace SELLit.Server.Features.Identity.Commands.Login;

public class LoginCommandRequestModel : IRequest<OneOf<LoginCommandOutputModel, InvalidLoginCredentials>>
{
    public string Username { get; set; }

    public string Password { get; set; }

    public class
        LoginCommandRequestHandler : IRequestHandler<LoginCommandRequestModel, OneOf<LoginCommandOutputModel, InvalidLoginCredentials>>
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

        public async Task<OneOf<LoginCommandOutputModel, InvalidLoginCredentials>> Handle(LoginCommandRequestModel request,
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

            var encryptedToken = this.identityService.GenerateJwtToken(user.Id, user.UserName, appSettings.Secret);

            return new LoginCommandOutputModel()
            {
                Token = encryptedToken
            };
        }
    }
}
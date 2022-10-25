using MediatR;
using Microsoft.AspNetCore.Identity;
using OneOf;
using OneOf.Types;
using SELLit.Server.Infrastructure;
using SELLit.Server.Infrastructure.Mapping.Interfaces;

namespace SELLit.Server.Features.Identity.Commands.Register;

public class RegisterCommandRequestModel : IRequest<OneOf<RegisterCommandOutputModel, InvalidLoginCredentials>>, IMapTo<User>
{
    public string Username { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public class RegisterCommandRequestModelHandler : IRequestHandler<RegisterCommandRequestModel,
        OneOf<RegisterCommandOutputModel, InvalidLoginCredentials>>
    {
        private readonly UserManager<User> userManager;

        public RegisterCommandRequestModelHandler(UserManager<User> userManager) => this.userManager = userManager;

        public async Task<OneOf<RegisterCommandOutputModel, InvalidLoginCredentials>> Handle(RegisterCommandRequestModel request,
            CancellationToken cancellationToken)
        {
            var user = new User
            {
                Email = request.Email,
                UserName = request.Username
            };

            var result = await this.userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new InvalidLoginCredentials(result.Errors);
            }

            return new RegisterCommandOutputModel();
        }
    }
}
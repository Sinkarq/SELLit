using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SELLit.Server.Infrastructure.Mapping.Interfaces;

namespace SELLit.Server.Features.Identity.Commands.Register;

public sealed class RegisterCommand : IRequest<OneOf<RegisterCommandResponseModel, InvalidLoginCredentials>>, IMapTo<User>
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string Email { get; set; } = default!;

    public sealed class RegisterCommandRequestModelHandler : IRequestHandler<RegisterCommand,
        OneOf<RegisterCommandResponseModel, InvalidLoginCredentials>>
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public RegisterCommandRequestModelHandler(UserManager<User> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async ValueTask<OneOf<RegisterCommandResponseModel, InvalidLoginCredentials>> Handle(RegisterCommand request,
            CancellationToken cancellationToken)
        {
            var user = this.mapper.Map<User>(request);

            var result = await this.userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new InvalidLoginCredentials(result.Errors);
            }

            return new RegisterCommandResponseModel();
        }
    }
}
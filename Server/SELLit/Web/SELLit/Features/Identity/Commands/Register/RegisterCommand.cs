using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OneOf;
using OneOf.Types;
using SELLit.Server.Infrastructure.Mapping.Interfaces;

namespace SELLit.Server.Features.Identity.Commands.Register;

public sealed class RegisterCommand : IRequest<OneOf<RegisterCommandResponseModel, InvalidLoginCredentials>>, IMapTo<User>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
    
    public string Username { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

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

        public async Task<OneOf<RegisterCommandResponseModel, InvalidLoginCredentials>> Handle(RegisterCommand request,
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
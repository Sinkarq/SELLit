using SELLit.Server.Features.Identity.Commands.Login;
using Swashbuckle.AspNetCore.Filters;

namespace SELLit.Server.Infrastructure.SwaggerConfiguration.SwaggerExamples.Identity;

public class LoginCommandResponseExample : IExamplesProvider<LoginCommandResponseModel>
{
    public LoginCommandResponseModel GetExamples() =>
        new()
        {
            Token = "example token"
        };
}
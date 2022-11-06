using SELLit.Server.Features.Identity.Commands.Login;
using Swashbuckle.AspNetCore.Filters;

namespace SELLit.Server.Infrastructure.SwaggerConfiguration.SwaggerExamples.Identity;

public class LoginCommandExample : IExamplesProvider<LoginCommand>
{
    public LoginCommand GetExamples() =>
        new()
        {
            Username = "John",
            Password = "password1234"
        };
}
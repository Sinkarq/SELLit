using SELLit.Server.Features.Identity.Commands.Register;
using Swashbuckle.AspNetCore.Filters;

namespace SELLit.Server.Infrastructure.SwaggerConfiguration.SwaggerExamples.Identity;

public class RegisterCommandExample : IExamplesProvider<RegisterCommand>
{
    public RegisterCommand GetExamples() =>
        new()
        {
            FirstName = "Albert",
            LastName = "Camus",
            Username = "albert69420",
            Password = "password1234",
            Email = "albert@gmail.com"
        };
}
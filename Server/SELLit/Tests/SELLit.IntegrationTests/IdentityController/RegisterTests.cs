using System.Net.Http.Json;
using SELLit.Server.Features.Identity.Commands.Register;
using SELLit.Server.Infrastructure;

namespace SELLit.IntegrationTests.IdentityController;

public class RegisterTests : IntegrationTestBase
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;
    private const string RegisterRoute = Routes.Identity.Register;

    public RegisterTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory) : base(factory)
    {
        Factory = factory;
    }
    
    [Fact]
    public async Task Register_Should_Return_200()
    {
        var httpClient = this.Factory.CreateClient();
        await httpClient.PostAsJsonShouldBeWithStatusCodeAsync(RegisterRoute, new RegisterCommand
        {
            FirstName = "Sinan",
            LastName = "Abdulgafurov",
            Username = "Sinkarcheto123",
            Password = "password1234",
            Email = "sinkarq1234565@gmail.com"
        }, HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Register_Should_Return_NotAvailable_Username_and_Email()
    {
        var httpClient = Factory.CreateClient();
        
        await httpClient.PostAsJsonAsync(RegisterRoute, new RegisterCommand
        {
            FirstName = "Sinan",
            LastName = "Abdulgafurov",
            Username = "Sinkarcheto",
            Password = "password1234",
            Email = "sinkarq123@gmail.com"
        });

        (await httpClient.DeserializePostAsJsonShouldBeWithStatusCodeAsync<ErrorModel, RegisterCommand>(
            Routes.Identity.Register, new RegisterCommand
            {
                FirstName = "Sinan",
                LastName = "Abdulgafurov",
                Username = "Sinkarcheto",
                Password = "password1234",
                Email = "sinkarq123@gmail.com"
            }, HttpStatusCode.BadRequest)).Errors.Should().HaveCount(2);
    }
}
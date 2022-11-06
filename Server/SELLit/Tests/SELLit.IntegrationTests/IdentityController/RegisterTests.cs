using System.Net.Http.Json;
using SELLit.Server.Features.Identity.Commands.Register;
using SELLit.Server.Infrastructure;

namespace SELLit.IntegrationTests.IdentityController;

[Collection(nameof(IntegrationTests))]
public class RegisterTests
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;
    private readonly HttpClient httpClient;
    private const string RegisterRoute = Routes.Identity.Register;

    public RegisterTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory)
    {
        Factory = factory;
        this.httpClient = factory.HttpClient;
    }

    [Fact]
    public async Task Register_Should_Return_204() =>
        await this.httpClient
            .WithNoAuthentication()
            .PostAsJsonShouldBeWithStatusCodeAsync(RegisterRoute, new RegisterCommand
            {
                FirstName = "Sinan",
                LastName = "Abdulgafurov",
                Username = "Sinkarcheto123",
                Password = "password1234",
                Email = "sinkarq1234565@gmail.com"
            }, HttpStatusCode.NoContent);

    [Fact]
    public async Task Register_Should_Return_NotAvailable_Username_and_Email()
    {
        await httpClient.PostAsJsonAsync(RegisterRoute, new RegisterCommand
        {
            FirstName = "Sinan",
            LastName = "Abdulgafurov",
            Username = "Sinkarcheto",
            Password = "password1234",
            Email = "sinkarq123@gmail.com"
        });

        (await httpClient
            .WithNoAuthentication()
            .DeserializePostAsJsonShouldBeWithStatusCodeAsync<ErrorModel, RegisterCommand>(
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
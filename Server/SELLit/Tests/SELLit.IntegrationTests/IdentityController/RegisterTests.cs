using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using SELLit.Data;
using SELLit.IntegrationTests.Setup;
using SELLit.Server;
using SELLit.Server.Features;
using SELLit.Server.Features.Identity.Commands.Register;
using SELLit.Server.Infrastructure;

namespace SELLit.IntegrationTests.IdentityController;

public class RegisterTests : IntegrationTestBase
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;

    public RegisterTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory) : base(factory)
    {
        Factory = factory;
    }
    
    [Fact]
    public async Task Register_Should_Return_200()
    {
        var httpClient = this.Factory.CreateClient();
        var response = (await httpClient.PostAsJsonAsync(Routes.Identity.Register, new RegisterCommand
        {
            FirstName = "Sinan",
            LastName = "Abdulgafurov",
            Username = "Sinkarcheto123",
            Password = "password1234",
            Email = "sinkarq1234565@gmail.com"
        })).StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Register_Should_Return_NotAvailable_Username_and_Email()
    {
        var httpClient = Factory.CreateClient();
        
        await httpClient.PostAsJsonAsync(Routes.Identity.Register, new RegisterCommand
        {
            FirstName = "Sinan",
            LastName = "Abdulgafurov",
            Username = "Sinkarcheto",
            Password = "password1234",
            Email = "sinkarq123@gmail.com"
        });

        var response = await httpClient.PostJsonAsyncShouldBeWithStatusCode<ErrorModel, RegisterCommand>(
            Routes.Identity.Register, new RegisterCommand
            {
                FirstName = "Sinan",
                LastName = "Abdulgafurov",
                Username = "Sinkarcheto",
                Password = "password1234",
                Email = "sinkarq123@gmail.com"
            }, HttpStatusCode.BadRequest);

        response.Errors.Should().HaveCount(2);
    }
}
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using SELLit.Common;
using SELLit.Server.Features;
using SELLit.Server.Features.Identity.Commands.Register;
using SELLit.Server.Infrastructure;
using Xunit;

namespace SELLit.Tests.IntegrationTests.IdentityController;

public class RegisterTests : IntegrationTest
{
    [Fact]
    public async Task Register_Should_Return_200()
    {
        var response = await this.HttpClient.PostAsJsonAsync(Routes.Identity.Register, new RegisterCommand
        {
            FirstName = "Sinan",
            LastName = "Abdulgafurov",
            Username = "Sinkarcheto123",
            Password = "password1234",
            Email = "sinkarq1234565@gmail.com"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Register_Should_Return_NotAvailable_Username_and_Email()
    {
        await this.HttpClient.PostAsJsonAsync(Routes.Identity.Register, new RegisterCommand
        {
            FirstName = "Sinan",
            LastName = "Abdulgafurov",
            Username = "Sinkarcheto",
            Password = "password1234",
            Email = "sinkarq123@gmail.com"
        });
        
        var response = await this.HttpClient.PostAsJsonAsync(Routes.Identity.Register, new RegisterCommand
        {
            FirstName = "Sinan",
            LastName = "Abdulgafurov",
            Username = "Sinkarcheto",
            Password = "password1234",
            Email = "sinkarq123@gmail.com"
        });

        var responseModel = await response.DeserializeHttpContentAsync<ErrorModel>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseModel.Errors.Should().HaveCount(2);
    }
}
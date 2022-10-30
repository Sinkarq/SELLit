using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using SELLit.Common;
using SELLit.Server.Features;
using SELLit.Server.Features.Identity.Commands.Login;
using SELLit.Server.Infrastructure;
using Xunit;

namespace SELLit.Tests.IntegrationTests.IdentityController;

public class LoginTests : IntegrationTest
{
    [Fact]
    public async Task LoginCommand_Should_Return_Valid_JWT_Token()
    {
        var response = await this.HttpClient.PostAsJsonAsync(Routes.Identity.Login, new LoginCommand()
        {
            Username = "John",
            Password = "password1234"
        });

        var responseModel = await response.DeserializeHttpContentAsync<LoginCommandResponseModel>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseModel.Token.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task LoginCommand_Should_Return_Bad_Request()
    {
        var response = await this.HttpClient.PostAsJsonAsync(Routes.Identity.Login, new LoginCommand()
        {
            Username = "John",
            Password = "password123"
        });
        
        var responseModel = await response.DeserializeHttpContentAsync<ErrorModel>();
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseModel.StatusCode.Should().Be(400);
        responseModel.Errors.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task LoginCommand_Should_Return_Bad_Request2()
    {
        var response = await this.HttpClient.PostAsJsonAsync(Routes.Identity.Login, new LoginCommand()
        {
            Username = "Joh",
            Password = "password1234"
        });
        
        var responseModel = await response.DeserializeHttpContentAsync<ErrorModel>();
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseModel.StatusCode.Should().Be(400);
        responseModel.Errors.Should().NotBeEmpty();
    }
}
using System.Net;
using System.Net.Http.Json;
using SELLit.Common;
using SELLit.Data;
using SELLit.IntegrationTests.Setup;
using SELLit.Server;
using SELLit.Server.Features;
using SELLit.Server.Features.Identity.Commands.Login;
using FluentAssertions;
using SELLit.Server.Infrastructure;

namespace SELLit.IntegrationTests.IdentityController;

public class LoginTests : IntegrationTestBase
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;

    public LoginTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory) : base(factory)
    {
        Factory = factory;
    }

    [Fact]
    public async Task LoginCommand_Should_Return_Valid_JWT_Token1()
    {
        var client = this.Factory.CreateClient();
        (await client
            .PostJsonAsyncShouldBeWithStatusCode<LoginCommandResponseModel, LoginCommand>(Routes.Identity.Login,
                new LoginCommand()
                {
                    Username = "John",
                    Password = "password1234"
                }, HttpStatusCode.OK)).Token.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task LoginCommand_Should_Return_BadRequest_WrongUsername()
    {
        var client = this.Factory.CreateClient();
        (await client.PostAsJsonAsync(Routes.Identity.Login, new LoginCommand()
        {
            Username = "WrongUsername",
            Password = "password1234"
        })).StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task LoginCommand_Should_Return_BadRequest_WrongPassword()
    {
        var client = this.Factory.CreateClient();
        (await client.PostAsJsonAsync(Routes.Identity.Login, new LoginCommand()
        {
            Username = "John",
            Password = "WrongPassword"
        })).StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
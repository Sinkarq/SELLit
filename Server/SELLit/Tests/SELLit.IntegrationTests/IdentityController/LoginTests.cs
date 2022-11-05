using SELLit.Server.Features.Identity.Commands.Login;

namespace SELLit.IntegrationTests.IdentityController;

[Collection(nameof(IntegrationTests))]
public class LoginTests
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;
    private readonly HttpClient httpClient;
    private const string LoginRoute = Routes.Identity.Login;

    public LoginTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory)
    {
        Factory = factory;
        this.httpClient = factory.HttpClient;
    }

    [Fact]
    public async Task LoginCommand_Should_Return_Valid_JWT_Token1() =>
        (await this.httpClient
            .WithNoAuthentication()
            .DeserializePostAsJsonShouldBeWithStatusCodeAsync<LoginCommandResponseModel, LoginCommand>(
                Routes.Identity.Login,
                new LoginCommand()
                {
                    Username = "John",
                    Password = "password1234"
                }, HttpStatusCode.OK)).Token.Should().NotBeEmpty();

    [Fact]
    public async Task LoginCommand_Should_Return_BadRequest_WrongUsername() =>
        await this.httpClient
            .WithNoAuthentication()
            .PostAsJsonShouldBeWithStatusCodeAsync(LoginRoute, new LoginCommand()
            {
                Username = "WrongUsername",
                Password = "password1234"
            }, HttpStatusCode.BadRequest);

    [Fact]
    public async Task LoginCommand_Should_Return_BadRequest_WrongPassword() =>
        await this.httpClient
            .WithNoAuthentication()
            .PostAsJsonShouldBeWithStatusCodeAsync(LoginRoute,
                new LoginCommand()
                {
                    Username = "John",
                    Password = "WrongPassword"
                }, HttpStatusCode.BadRequest);
}
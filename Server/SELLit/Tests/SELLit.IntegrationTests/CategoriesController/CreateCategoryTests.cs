using SELLit.Server.Features.Categories.Commands.Create;
using SELLit.Server.Infrastructure;

namespace SELLit.IntegrationTests.CategoriesController;

[Collection(nameof(IntegrationTests))]
public class CreateCategoryTests
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;
    private const string CreateCategoryRoute = Routes.Categories.Create;
    private readonly HttpClient httpClient;

    public CreateCategoryTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory)
    {
        Factory = factory;
        this.httpClient = factory.HttpClient;
    }

    [Fact]
    public async Task Should_Succeed()
    {
        var responseMessage = await this.httpClient
            .WithAdminAuthentication()
            .PostAsJsonShouldBeWithStatusCodeAsync(CreateCategoryRoute,
                new CreateCategoryCommand
                {
                    Name = new Faker().Person.FirstName
                }, HttpStatusCode.Created);

        var url = responseMessage.Headers.Location.LocalPath;

        await httpClient.GetShouldBeWithStatusCodeAsync(url, HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Return_Bad_Request_UniqueName_Required() =>
        (await this.httpClient
            .WithAdminAuthentication()
            .DeserializePostAsJsonShouldBeWithStatusCodeAsync<ErrorModel, CreateCategoryCommand>(CreateCategoryRoute,
                new CreateCategoryCommand
                {
                    Name = Factory.ActiveCategories[0].Name
                }, HttpStatusCode.BadRequest)).Errors.Should().NotBeEmpty();

    [Fact]
    public async Task Should_Return_Unauthorized() =>
        await httpClient
            .WithNoAuthentication()
            .PostAsJsonShouldBeWithStatusCodeAsync(
                CreateCategoryRoute,
                new CreateCategoryCommand
                {
                    Name = Factory.ActiveCategories[0].Name
                }, HttpStatusCode.Unauthorized);

    [Fact]
    public async Task Should_Return_Forbidden() =>
        await httpClient
            .WithDefaultAuthentication()
            .PostAsJsonShouldBeWithStatusCodeAsync(
                CreateCategoryRoute,
                new CreateCategoryCommand
                {
                    Name = Factory.ActiveCategories[0].Name
                }, HttpStatusCode.Forbidden);
}
using SELLit.Common;
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
        var name = new Faker().Person.FirstName;
        var responseMessage = await this.httpClient
            .WithAdminAuthentication()
            .PostAsJsonShouldBeWithStatusCodeAsync(CreateCategoryRoute,
                new CreateCategoryCommand
                {
                    Name = name
                }, HttpStatusCode.Created);

        var url = responseMessage.Headers.Location.LocalPath;
        var category = await responseMessage.DeserializeHttpContentAsync<CreateCategoryCommandResponseModel>();
        category.Id.Should().NotBeNullOrEmpty();
        category.Name.Should().Be(name);

        await httpClient.GetShouldBeWithStatusCodeAsync(url, HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Return_Bad_Request_UniqueName_Required() =>
        (await this.httpClient
            .WithAdminAuthentication()
            .DeserializePostAsJsonShouldBeWithStatusCodeAsync<ErrorResponse, CreateCategoryCommand>(CreateCategoryRoute,
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

internal sealed class CreateCategoryCommandResponseModel
{
    public string Id { get; set; }
    
    public string Name { get; set; }
}
using SELLit.Server.Features.Categories.Commands.Create;
using SELLit.Server.Infrastructure;

namespace SELLit.IntegrationTests.CategoriesController;

public class CreateCategoryTests : IntegrationTestBase
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;
    private const string CreateCategoryRoute = Routes.Categories.Create;

    public CreateCategoryTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory) : base(factory)
    {
        Factory = factory;
    }
    
    [Fact]
    public async Task Should_Succeed()
    {
        var httpClient = Factory.CreateClient();

        await AuthenticateAdminAsync(httpClient);

        var responseMessage = await httpClient.PostAsJsonShouldBeWithStatusCodeAsync(CreateCategoryRoute,
            new CreateCategoryCommand
            {
                Name = Guid.NewGuid().ToString()
            }, HttpStatusCode.Created);

        var url = responseMessage.Headers.Location.LocalPath;
        
        await httpClient.GetShouldBeWithStatusCodeAsync(url, HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Return_Bad_Request_UniqueName_Required()
    {
        var httpClient = Factory.CreateClient();

        await AuthenticateAdminAsync(httpClient);

        (await httpClient.DeserializePostAsJsonShouldBeWithStatusCodeAsync<ErrorModel, CreateCategoryCommand>(CreateCategoryRoute,
            new CreateCategoryCommand
            {
                Name = "firstCategory"
            }, HttpStatusCode.BadRequest)).Errors.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task Should_Return_Unauthorized()
    {
        var httpClient = Factory.CreateClient();

        await httpClient.PostAsJsonShouldBeWithStatusCodeAsync(
            CreateCategoryRoute,
            new CreateCategoryCommand
            {
                Name = "firstCategory"
            }, HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Should_Return_Forbidden()
    {
        var httpClient = Factory.CreateClient();

        await AuthenticateAsync(httpClient);

        await httpClient.PostAsJsonShouldBeWithStatusCodeAsync(
            CreateCategoryRoute,
            new CreateCategoryCommand
            {
                Name = "firstCategory"
            }, HttpStatusCode.Forbidden);
    }
}
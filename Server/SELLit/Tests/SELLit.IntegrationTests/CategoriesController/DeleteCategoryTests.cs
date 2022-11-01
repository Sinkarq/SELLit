using Microsoft.Extensions.DependencyInjection;

namespace SELLit.IntegrationTests.CategoriesController;

public class DeleteCategoryTests : IntegrationTestBase
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;

    public DeleteCategoryTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory) : base(factory)
    {
        Factory = factory;
    }

    [Fact]
    public async Task Should_Delete_And_Return_OK()
    {
        var httpClient = Factory.CreateClient();

        await AuthenticateAdminAsync(httpClient);

        var hashids = this.ServiceProvider.GetService<IHashids>();
        var id = hashids?.Encode(1);

        await httpClient.DeleteShouldBeWithStatusCodeAsync(Routes.Categories.DeleteById(id), HttpStatusCode.OK);

        await httpClient.GetShouldBeWithStatusCodeAsync(Routes.Categories.GetById(id), HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task Should_Return_NotFound()
    {
        var httpClient = Factory.CreateClient();

        await AuthenticateAdminAsync(httpClient);

        var hashids = this.ServiceProvider.GetService<IHashids>();
        var id = hashids?.Encode(69);

        await httpClient.DeleteShouldBeWithStatusCodeAsync(Routes.Categories.DeleteById(id), HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task Should_Return_Unauthorized_When_Not_LoggedIn()
    {
        var httpClient = Factory.CreateClient();

        var hashids = this.ServiceProvider.GetService<IHashids>();
        var id = hashids?.Encode(1);

        await httpClient.DeleteShouldBeWithStatusCodeAsync(Routes.Categories.DeleteById(id), HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Should_Return_Forbidden()
    {
        var httpClient = Factory.CreateClient();

        await AuthenticateAsync(httpClient);

        var hashids = this.ServiceProvider.GetService<IHashids>();
        var id = hashids?.Encode(1);

        await httpClient.DeleteShouldBeWithStatusCodeAsync(Routes.Categories.DeleteById(id), HttpStatusCode.Forbidden);
    }
}
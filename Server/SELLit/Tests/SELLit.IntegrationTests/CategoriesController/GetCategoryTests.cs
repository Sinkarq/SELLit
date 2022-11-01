using Microsoft.Extensions.DependencyInjection;

namespace SELLit.IntegrationTests.CategoriesController;

public class GetCategoryTests : IntegrationTestBase
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;

    public GetCategoryTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory) : base(factory)
    {
        Factory = factory;
    }

    [Fact]
    public async Task GetCategory_Returns_OK()
    {
        var httpClient = Factory.CreateClient();

        var hashids = this.ServiceProvider.GetService<IHashids>();
        var id = hashids?.Encode(1);


        (await httpClient.DeserializeGetShouldBeWithStatusCodeAsync<GetCategoryQueryTestResponseModel>(
            Routes.Categories.GetById(id), HttpStatusCode.OK)).Id.Should().Be(id);
    }
    
    [Fact]
    public async Task GetCategory_Returns_NotFound()
    {
        var httpClient = Factory.CreateClient();

        var hashids = this.ServiceProvider.GetService<IHashids>();
        var id = hashids?.Encode(69);


        await httpClient.DeserializeGetShouldBeWithStatusCodeAsync<GetCategoryQueryTestResponseModel>(
            Routes.Categories.GetById(id), HttpStatusCode.NotFound);
    }
}

public class GetCategoryQueryTestResponseModel
{
    public string Id { get; set; }
    public string Name { get; set; }
}
namespace SELLit.IntegrationTests.CategoriesController;

public class GetAllCategoriesTests : IntegrationTestBase
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;
    private const string GetAllRoute = Routes.Categories.GetAll;

    public GetAllCategoriesTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory) : base(factory)
    {
        Factory = factory;
    }

    [Fact]
    public async Task GetAll_Returns_Two_Categories()
    {
        var httpClient = Factory.CreateClient();

        (await httpClient.DeserializeGetShouldBeWithStatusCodeAsync<List<GetAllCategoriesQueryTestResponseModel>>(
            GetAllRoute, HttpStatusCode.OK))
            .Should().NotContainNulls()
            .And.HaveCount(2);
    }
}

internal sealed class GetAllCategoriesQueryTestResponseModel
{
    public string Id { get; set; }
    
    public string Name { get; set; }
}
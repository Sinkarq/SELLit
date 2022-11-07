namespace SELLit.IntegrationTests.CategoriesController;

[Collection(nameof(IntegrationTests))]
public class GetAllCategoriesTests
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;
    private readonly HttpClient httpClient;
    private const string GetAllRoute = Routes.Categories.GetAll;

    public GetAllCategoriesTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory)
    {
        Factory = factory;
        this.httpClient = factory.HttpClient; 
    }

    [Fact]
    public async Task GetAll_Returns_Two_Categories() =>
        (await httpClient
            .WithNoAuthentication()
            .DeserializeGetShouldBeWithStatusCodeAsync<List<GetAllCategoriesQueryTestResponseModel>>(
                GetAllRoute,
                HttpStatusCode.OK))
        .Should().NotContainNulls()
        .And.HaveCountGreaterThan(0);
}

internal sealed class GetAllCategoriesQueryTestResponseModel
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;
}
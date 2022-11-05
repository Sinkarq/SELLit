namespace SELLit.IntegrationTests.CategoriesController;

[Collection(nameof(IntegrationTests))]
public class GetCategoryTests
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;
    private readonly IHashids hashids;
    private readonly HttpClient httpClient;

    public GetCategoryTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory)
    {
        Factory = factory;
        this.httpClient = factory.HttpClient;
        this.hashids = factory.Hashids;
    }

    [Fact]
    public async Task GetCategory_Returns_OK()
    {
        var id = this.hashids.Encode(2);

        (await httpClient
            .WithNoAuthentication()
            .DeserializeGetShouldBeWithStatusCodeAsync<GetCategoryQueryTestResponseModel>(
                Routes.Categories.GetById(id),
                HttpStatusCode.OK)).Id.Should().Be(id);
    }

    [Fact]
    public async Task GetCategory_Returns_NotFound() =>
        await httpClient
            .WithNoAuthentication()
            .DeserializeGetShouldBeWithStatusCodeAsync<GetCategoryQueryTestResponseModel>(
                Routes.Categories.GetById(this.hashids.Encode(69)),
                HttpStatusCode.NotFound);
}

public class GetCategoryQueryTestResponseModel
{
    public string Id { get; set; }
    public string Name { get; set; }
}
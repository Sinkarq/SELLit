using SELLit.Server.Features.Products.Queries.GetAll;

namespace SELLit.IntegrationTests.ProductsController;

[Collection(nameof(IntegrationTests))]
public class GetAllProductTests
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;
    private readonly HttpClient HttpClient;

    public GetAllProductTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory)
    {
        this.Factory = factory;
        this.HttpClient = factory.HttpClient;
    }

    [Fact]
    public async Task GetAllProducts_Returns_OK()
    {
        var products = await this.HttpClient
            .DeserializeGetShouldBeWithStatusCodeAsync<List<GetAllProductsQueryTestResponseModel>>(
                Routes.Products.GetAll,
                HttpStatusCode.OK);

        products.Should().HaveCountGreaterThan(1);
        foreach (var product in products)
        {
            product.Id.Should().NotBeNullOrEmpty();
            product.Title.Should().NotBeNullOrEmpty();
        }
    }
}

public class GetAllProductsQueryTestResponseModel
{
    public string Id { get; set; }

    public string Title { get; set; }
}
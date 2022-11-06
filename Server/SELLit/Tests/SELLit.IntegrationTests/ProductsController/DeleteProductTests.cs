using SELLit.Data.Models;

namespace SELLit.IntegrationTests.ProductsController;

[Collection(nameof(IntegrationTests))]
public class DeleteProductTests
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> factory;
    private readonly HttpClient httpClient;
    private readonly CreateProductTestCommand defaultCommand;

    public DeleteProductTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory)
    {
        this.factory = factory;
        this.httpClient = factory.HttpClient;
        this.defaultCommand = new Faker<CreateProductTestCommand>()
            .RuleFor(x => x.Title, x => x.Person.FirstName)
            .RuleFor(x => x.Description, x => x.Lorem.Sentence())
            .RuleFor(x => x.Location, x => x.Address.FullAddress())
            .RuleFor(x => x.PhoneNumber, x => x.Phone.PhoneNumber())
            .RuleFor(x => x.Price, _ => 69.420)
            .RuleFor(x => x.CategoryId, _ => factory.ActiveCategories[0].Id)
            .RuleFor(x => x.DeliveryResponsibility, _ => DeliveryResponsibility.Buyer);
    }

    [Fact]
    public async Task DeleteProduct_Should_Return_OK()
    {
        var response = await this.httpClient
            .WithDefaultAuthentication()
            .PostAsJsonShouldBeWithStatusCodeAsync(Routes.Products.Create, defaultCommand, HttpStatusCode.Created);

        var product = await this.httpClient
            .DeserializeGetShouldBeWithStatusCodeAsync<GetProductQueryTestResponseModel>(
                response.Headers.Location!.LocalPath,
                HttpStatusCode.OK);

        await this.httpClient
            .WithDefaultAuthentication()
            .DeleteShouldBeWithStatusCodeAsync(Routes.Products.DeleteById(product.Id), HttpStatusCode.OK);

        await this.httpClient
            .DeserializeGetShouldBeWithStatusCodeAsync<GetProductQueryTestResponseModel>(
                response.Headers.Location!.LocalPath,
                HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteProduct_Should_Return_NotFound() =>
        await this.httpClient
            .WithDefaultAuthentication()
            .DeleteShouldBeWithStatusCodeAsync(
                Routes.Products.DeleteById(factory.Hashids.Encode(69)),
                HttpStatusCode.NotFound);

    [Fact]
    public async Task DeleteProduct_Should_Return_Forbidden()
    {
        var response = await this.httpClient
            .WithDefaultAuthentication()
            .PostAsJsonShouldBeWithStatusCodeAsync(Routes.Products.Create, defaultCommand, HttpStatusCode.Created);

        var product = await this.httpClient
            .DeserializeGetShouldBeWithStatusCodeAsync<GetProductQueryTestResponseModel>(
                response.Headers.Location!.LocalPath,
                HttpStatusCode.OK);

        await this.httpClient
            .WithAdminAuthentication()
            .DeleteShouldBeWithStatusCodeAsync(Routes.Products.DeleteById(product.Id), HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteProduct_Should_Return_Unauthorized() =>
        await this.httpClient
            .WithNoAuthentication()
            .DeleteShouldBeWithStatusCodeAsync(
                Routes.Products.DeleteById(factory.Hashids.Encode(1)),
                HttpStatusCode.Unauthorized);
}
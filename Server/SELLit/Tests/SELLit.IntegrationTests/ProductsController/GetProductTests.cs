using CommunityToolkit.Diagnostics;
using SELLit.Data.Models;

namespace SELLit.IntegrationTests.ProductsController;

[Collection(nameof(IntegrationTests))]
public class GetProductTests
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;
    private readonly HttpClient httpClient;
    private readonly CreateProductTestCommand defaultCommand;

    public GetProductTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory)
    {
        Factory = factory;
        this.httpClient = factory.HttpClient;
        this.defaultCommand = new Faker<CreateProductTestCommand>()
            .RuleFor(x => x.Title, x => x.Person.FirstName)
            .RuleFor(x => x.Description, x => x.Lorem.Sentence())
            .RuleFor(x => x.Location, x => x.Address.FullAddress())
            .RuleFor(x => x.PhoneNumber, x => x.Phone.PhoneNumber())
            .RuleFor(x => x.Price, _ => 69.420)
            .RuleFor(x => x.CategoryId, _ => Factory.ActiveCategories[0].Id)
            .RuleFor(x => x.DeliveryResponsibility, _ => DeliveryResponsibility.Buyer);
    }

    [Fact]
    public async Task Get_ReturnsProduct_WhenProductExists()
    {
        var response = await this.httpClient
            .WithDefaultAuthentication()
            .PostAsJsonShouldBeWithStatusCodeAsync(Routes.Products.Create, defaultCommand, HttpStatusCode.Created);

        var locationHeader = response.Headers.Location;
        Guard.IsNotNull(locationHeader, "Location header is not present in the response headers");

        var product = await this.httpClient
            .DeserializeGetShouldBeWithStatusCodeAsync<GetProductQueryTestResponseModel>(
                locationHeader.LocalPath,
                HttpStatusCode.OK);

        product.Title.Should().Be(defaultCommand.Title);
        product.Description.Should().Be(defaultCommand.Description);
        product.Location.Should().Be(defaultCommand.Location);
        product.Price.Should().Be(defaultCommand.Price);
        product.DeliveryResponsibility.Should().Be(defaultCommand.DeliveryResponsibility);
        product.PhoneNumber.Should().Be(defaultCommand.PhoneNumber);
        product.CategoryName.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task Get_404_WhenProductDoesntExists() =>
        await this.httpClient
            .GetShouldBeWithStatusCodeAsync(
                Factory.Hashids.Encode(69),
                HttpStatusCode.NotFound);
}

public class GetProductQueryTestResponseModel
{
    public string Id { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Location { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public double Price { get; set; }

    public DeliveryResponsibility DeliveryResponsibility { get; set; }
    
    public string CategoryName { get; set; } = default!;
}
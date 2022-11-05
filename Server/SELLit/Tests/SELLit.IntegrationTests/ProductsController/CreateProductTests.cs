using Bogus;
using SELLit.Data.Models;
using SELLit.Server.Features.Products.Commands.Create;

namespace SELLit.IntegrationTests.ProductsController;

[Collection(nameof(IntegrationTests))]
public class CreateProductTests
{
    private static IntegrationTestFactory<Startup, ApplicationDbContext> Factory;
    private const string CreateProductRoute = Routes.Products.Create;
    private readonly HttpClient httpClient;
    private readonly CreateProductTestCommand defaultCommand;

    public CreateProductTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory)
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
    public async Task Should_Return_201_Created()
    {
        var response = await this.httpClient
            .WithDefaultAuthentication()
            .PostAsJsonShouldBeWithStatusCodeAsync(CreateProductRoute, defaultCommand, HttpStatusCode.Created);

        var product = await this.httpClient
            .DeserializeGetShouldBeWithStatusCodeAsync<GetProductQueryTestResponseModel>(
                response.Headers.Location!.LocalPath,
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
    public async Task Should_Return_Unauthorized() =>
        await this.httpClient
            .WithNoAuthentication()
            .PostAsJsonShouldBeWithStatusCodeAsync(CreateProductRoute, defaultCommand, HttpStatusCode.Unauthorized);
}

internal sealed record CreateProductTestCommand
{
    public string Title { get; set; }

    public string Description { get; set; }

    public string Location { get; set; }

    public string PhoneNumber { get; set; }

    public double Price { get; set; }

    public DeliveryResponsibility DeliveryResponsibility { get; set; }
    
    public int CategoryId { get; set; }
}
using SELLit.Data.Models;

namespace SELLit.IntegrationTests.ProductsController;

[Collection(nameof(IntegrationTests))]
public class UpdateProductTests
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;
    private readonly HttpClient httpClient;
    private readonly CreateProductTestCommand createCommand;
    private readonly UpdateProductTestCommand updateCommand;

    public UpdateProductTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory)
    {
        this.Factory = factory;
        this.httpClient  = factory.HttpClient;
        var lastProduct = factory.ActiveProducts.Last();
        this.createCommand = new Faker<CreateProductTestCommand>()
            .RuleFor(x => x.Title, x => x.Person.FirstName)
            .RuleFor(x => x.Description, x => x.Lorem.Sentence())
            .RuleFor(x => x.Location, x => x.Address.FullAddress())
            .RuleFor(x => x.PhoneNumber, x => x.Phone.PhoneNumber())
            .RuleFor(x => x.Price, _ => 69.420)
            .RuleFor(x => x.CategoryId, _ => factory.ActiveCategories[0].Id)
            .RuleFor(x => x.DeliveryResponsibility, _ => DeliveryResponsibility.Buyer);
        this.updateCommand = new Faker<UpdateProductTestCommand>()
            .RuleFor(x => x.Id, _ => lastProduct.Id)
            .RuleFor(x => x.Title, x => x.Person.FirstName)
            .RuleFor(x => x.Description, x => x.Lorem.Sentence())
            .RuleFor(x => x.Location, x => x.Address.FullAddress())
            .RuleFor(x => x.PhoneNumber, x => x.Phone.PhoneNumber())
            .RuleFor(x => x.Price, _ => 69.420)
            .RuleFor(x => x.DeliveryResponsibility, _ => DeliveryResponsibility.Buyer);
    }

    [Fact]
    public async Task Update_Should_Return_OK()
    {
        var response = await this.httpClient
            .WithDefaultAuthentication()
            .PostAsJsonShouldBeWithStatusCodeAsync(Routes.Products.Create, createCommand, HttpStatusCode.Created);

        var product = await this.httpClient
            .DeserializeGetShouldBeWithStatusCodeAsync<GetProductQueryTestResponseModel>(
                response.Headers.Location!.LocalPath,
                HttpStatusCode.OK);
        
        await this.httpClient
            .WithDefaultAuthentication()
            .PutAsJsonShouldBeWithStatusCodeAsync(Routes.Products.Update, updateCommand with {Id = Factory.Hashids.Decode(product.Id)[0]}, HttpStatusCode.OK);
        
        var updatedProduct = await this.httpClient
            .DeserializeGetShouldBeWithStatusCodeAsync<GetProductQueryTestResponseModel>(
                Routes.Products.GetById(product.Id),
                HttpStatusCode.OK);
        
        updatedProduct.Title.Should().Be(updateCommand.Title);
        updatedProduct.Description.Should().Be(updateCommand.Description);
        updatedProduct.Location.Should().Be(updateCommand.Location);
        updatedProduct.Price.Should().Be(updateCommand.Price);
        updatedProduct.DeliveryResponsibility.Should().Be(updateCommand.DeliveryResponsibility);
        updatedProduct.PhoneNumber.Should().Be(updateCommand.PhoneNumber);
        updatedProduct.CategoryName.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Update_Should_Return_Forbidden()
    {
        var response = await this.httpClient
            .WithDefaultAuthentication()
            .PostAsJsonShouldBeWithStatusCodeAsync(Routes.Products.Create, createCommand, HttpStatusCode.Created);

        var product = await this.httpClient
            .DeserializeGetShouldBeWithStatusCodeAsync<GetProductQueryTestResponseModel>(
                response.Headers.Location!.LocalPath,
                HttpStatusCode.OK);
        
        await this.httpClient
            .WithAdminAuthentication()
            .PutAsJsonShouldBeWithStatusCodeAsync(
                Routes.Products.Update, updateCommand with {Id = Factory.Hashids.Decode(product.Id)[0]},
                HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Update_Should_Return_Unauthorized()
    {
        var response = await this.httpClient
            .WithDefaultAuthentication()
            .PostAsJsonShouldBeWithStatusCodeAsync(Routes.Products.Create, createCommand, HttpStatusCode.Created);

        var product = await this.httpClient
            .DeserializeGetShouldBeWithStatusCodeAsync<GetProductQueryTestResponseModel>(
                response.Headers.Location!.LocalPath,
                HttpStatusCode.OK);
        
        await this.httpClient
            .WithNoAuthentication()
            .PutAsJsonShouldBeWithStatusCodeAsync(
                Routes.Products.Update, updateCommand with {Id = Factory.Hashids.Decode(product.Id)[0]},
                HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Update_Should_Return_NotFound() =>
        await this.httpClient
            .WithDefaultAuthentication()
            .PutAsJsonShouldBeWithStatusCodeAsync(
                Routes.Products.Update, updateCommand with {Id = 69},
                HttpStatusCode.NotFound);
}

public record UpdateProductTestCommand
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public string PhoneNumber { get; set; }
    public double Price { get; set; }
    public DeliveryResponsibility DeliveryResponsibility { get; set; }
}

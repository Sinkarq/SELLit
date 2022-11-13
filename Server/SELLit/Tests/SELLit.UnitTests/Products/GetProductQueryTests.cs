using SELLit.Server.Features.Products.Queries.Get;

namespace SELLit.UnitTests.Products;

public class GetProductQueryTests
{
    private readonly IDeletableEntityRepository<Product> repository = Substitute.For<IDeletableEntityRepository<Product>>();
    private readonly IFixture fixture = new Fixture();

    public GetProductQueryTests()
    {
        var category = new Category
        {
            Id = 69,
            Name = fixture.Create<string>()
        };
        fixture.Customize<Product>(x => x.With(prop => prop.Category, category));
        fixture.Customize<Product>(x => x.With(prop => prop.CategoryId, category.Id));
    }
    
    [Fact]
    public async Task GetProductQueryReturnsValidCategory()
    {
        var products = fixture.Create<List<Product>>();
        repository.AllAsNoTracking().Returns(products.BuildMock());

        var query = new GetProductQuery
        {
            Id = products[0].Id
        };

        var handler = new GetProductQuery.GetProductQueryHandler(repository);
        var result = await handler.Handle(query, new CancellationToken());

        result.Id.Should().Be(products[0].Id);
        result.Title.Should().Be(products[0].Title);
        result.Description.Should().Be(products[0].Description);
        result.Location.Should().Be(products[0].Location);
        result.PhoneNumber.Should().Be(products[0].PhoneNumber);
        result.Price.Should().Be(products[0].Price);
        result.DeliveryResponsibility.Should().Be(products[0].DeliveryResponsibility);
        result.CategoryName.Should().Be(products[0].Category.Name);
    }
    
    [Fact]
    public async Task ShouldReturnNullWhenNotFound()
    {
        var products = fixture.Create<List<Product>>();
        repository.AllAsNoTracking().Returns(products.BuildMock());

        var query = new GetProductQuery
        {
            Id = products[0].Id + 69
        };

        var handler = new GetProductQuery.GetProductQueryHandler(repository);
        var result = await handler.Handle(query, new CancellationToken());

        result.Should().BeNull();
    }
}
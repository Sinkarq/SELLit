using SELLit.Server.Features.Products.Queries.GetAll;

namespace SELLit.UnitTests.Products;

public class GetAllProductsQueryTests
{
    private readonly IDeletableEntityRepository<Product> repository = Substitute.For<IDeletableEntityRepository<Product>>();
    private readonly IFixture fixture = new Fixture();
    
    [Fact]
    public async Task Returns_Valid_Category_Collection()
    {
        var categories = fixture.Create<List<Product>>();
        var expectedResult = categories.Select(x => new GetAllProductsQueryResponseModel()
        {
            Id = x.Id,
            Title = x.Title
        }).ToList();
        
        var query = GetAllProductsQuery.Instance;

        repository.AllAsNoTracking().Returns(categories.BuildMock());

        var handler = new GetAllProductsQuery.GetAllProductsQueryHandler(repository);
        var result = await handler.Handle(query, new CancellationToken());
        var resultList = result.ToList();

        for (var i = 0; i < resultList.Count; i++)
        {
            resultList[i].Title.Should().Be(expectedResult[i].Title);
            resultList[i].Id.Should().Be(expectedResult[i].Id);
        }
    }
}
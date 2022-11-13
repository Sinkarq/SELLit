using SELLit.Server.Features.Categories.Queries.Get;

namespace SELLit.UnitTests.Categories;

public class GetCategoryQueryTests
{
    private readonly IDeletableEntityRepository<Category> repository = Substitute.For<IDeletableEntityRepository<Category>>();
    private readonly IFixture fixture = new Fixture();
    
    [Fact]
    public async Task GetCategoryQueryReturnsValidCategory()
    {
        
        var categories = fixture.Create<List<Category>>();
        var category = categories[0];
        
        var query = new GetCategoryQuery()
        {
            Id = category.Id
        };

        repository.AllAsNoTracking().Returns(categories.BuildMock());

        var handler = new GetCategoryQuery.GetCategoryQueryHandler(repository);
        var result = await handler.Handle(query, new CancellationToken());

        result.Id.Should().Be(category.Id);
        result.Name.Should().Be(category.Name);
    }
    
    [Fact]
    public async Task GetCategoryQueryReturnsWhenCategoryDoesntExist()
    {
        var categories = fixture.Create<List<Category>>();
        var category = categories[0];
        
        var query = new GetCategoryQuery()
        {
            Id = category.Id + 69
        };

        repository.AllAsNoTracking().Returns(categories.BuildMock());

        var handler = new GetCategoryQuery.GetCategoryQueryHandler(repository);
        var result = await handler.Handle(query, new CancellationToken());

        result.Should().BeNull();
    }
}
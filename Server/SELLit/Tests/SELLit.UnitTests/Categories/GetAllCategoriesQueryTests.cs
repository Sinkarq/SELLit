using SELLit.Server.Features.Categories.Queries.GetAll;

namespace SELLit.UnitTests.Categories;

public class GetAllCategoriesQueryTests
{
    private readonly IDeletableEntityRepository<Category> repository = Substitute.For<IDeletableEntityRepository<Category>>();
    private readonly IFixture fixture = new Fixture();

    [Fact]
    public async Task Returns_Valid_Category_Collection()
    {
        var categories = fixture.Create<List<Category>>();
        var expectedResult = categories.Select(x => new GetAllCategoriesQueryResponseModel()
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();
        
        var query = GetAllCategoriesQuery.Instance;

        repository.AllAsNoTracking().Returns(categories.BuildMock());

        var handler = new GetAllCategoriesQuery.GetAllCategoriesQueryHandler(repository);
        var result = await handler.Handle(query, new CancellationToken());
        var resultList = result.ToList();

        for (var i = 0; i < resultList.Count; i++)
        {
            resultList[i].Name.Should().Be(expectedResult[i].Name);
            resultList[i].Id.Should().Be(expectedResult[i].Id);
        }
    }
}
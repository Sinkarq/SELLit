using Microsoft.Extensions.Logging;
using SELLit.Server.Features.Categories.Commands.Create;

namespace SELLit.UnitTests.Categories;

public class CreateCategoryCommandTests
{
    private readonly IDeletableEntityRepository<Category> repository 
        = Substitute.For<IDeletableEntityRepository<Category>>();

    private readonly ILogger<CreateCategoryCommand.CreateCategoryCommandHandler> logger
        = Substitute.For<ILogger<CreateCategoryCommand.CreateCategoryCommandHandler>>();
    
    private readonly IFixture fixture = new Fixture();
    
    public CreateCategoryCommandTests()
    {
        fixture.Customize<Category>(c => c.With(p => p.IsDeleted, false));
    }

    [Fact]
    public async Task ShouldReturnValidCreateCategoryResponseModelWhenSuccessful()
    {
        var categories = fixture.Create<List<Category>>();
        var categoriesQueryable = categories.BuildMock();
        repository.AllAsNoTracking().Returns(categoriesQueryable);

        var command = new CreateCategoryCommand() {Name = "MyName"};
        var handler = new CreateCategoryCommand.CreateCategoryCommandHandler(repository, logger);
        var result = await handler.Handle(command, new CancellationToken());
        var responseModel = result.AsT0;
        
        result.IsT0.Should().BeTrue();
        logger.Received(1).BeginScope(Arg.Any<Dictionary<string, object>>());
        responseModel.Name.Should().Be("MyName");
    }

    [Fact]
    public async Task ShouldReturnUniqueConstraintErrorWhenNameAlreadyExists()
    {
        var categories = fixture.Create<List<Category>>();
        var categoriesQueryable = categories.BuildMock();
        repository.AllAsNoTracking().Returns(categoriesQueryable);

        var command = new CreateCategoryCommand {Name = categories[0].Name};
        var handler = new CreateCategoryCommand.CreateCategoryCommandHandler(repository, logger);
        var result = await handler.Handle(command, new CancellationToken());

        result.IsT1.Should().BeTrue();
    }
}
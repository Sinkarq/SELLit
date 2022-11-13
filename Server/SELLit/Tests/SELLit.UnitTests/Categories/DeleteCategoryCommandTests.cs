using Microsoft.Extensions.Logging;
using SELLit.Server.Features.Categories.Commands.Delete;

namespace SELLit.UnitTests.Categories;

public class DeleteCategoryCommandTests
{
    private readonly IDeletableEntityRepository<Category> repository 
        = Substitute.For<IDeletableEntityRepository<Category>>();

    private readonly ILogger<DeleteCategoryCommand.DeleteCategoryCommandHandler> logger
        = Substitute.For<ILogger<DeleteCategoryCommand.DeleteCategoryCommandHandler>>();
    
    private readonly IFixture fixture = new Fixture();

    public DeleteCategoryCommandTests()
    {
        fixture.Customize<Category>(c => c.With(p => p.IsDeleted, false));
    }

    [Fact]
    public async Task ShouldDeleteSuccessfully()
    {
        var categories = fixture.Create<List<Category>>();

        var dbSet = categories.BuildMock();

        repository.AllAsNoTracking().Returns(dbSet);
        
        var command = new DeleteCategoryCommand()
        {
            Id = categories[0].Id
        };

        var handler = new DeleteCategoryCommand.DeleteCategoryCommandHandler(repository, logger);
        var result = await handler.Handle(command, new CancellationToken());

        logger.Received(1).BeginScope(Arg.Any<Dictionary<string, object>>());
        result.IsT0.Should().BeTrue();
    }
    
    [Fact]
    public async Task ShouldReturnUniqueConstraintErrorWhenNameAlreadyExists()
    {
        var categories = fixture.Create<List<Category>>();

        var dbSet = categories.BuildMock();

        repository.AllAsNoTracking().Returns(dbSet);
        
        var command = new DeleteCategoryCommand()
        {
            Id = categories[0].Id + 69
        };

        var handler = new DeleteCategoryCommand.DeleteCategoryCommandHandler(repository, logger);
        var result = await handler.Handle(command, new CancellationToken());
        
        logger.Received(0).BeginScope(Arg.Any<Dictionary<string, object>>());
        result.IsT1.Should().BeTrue();
    }
}
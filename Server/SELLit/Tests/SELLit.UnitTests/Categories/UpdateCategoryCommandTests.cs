using Microsoft.Extensions.Logging;
using SELLit.Server.Features.Categories.Commands.Update;

namespace SELLit.UnitTests.Categories;

public class UpdateCategoryCommandTests
{
    private readonly IDeletableEntityRepository<Category> repository 
        = Substitute.For<IDeletableEntityRepository<Category>>();

    private readonly ILogger<UpdateCategoryCommand.UpdateCategoryCommandHandler> logger
        = Substitute.For<ILogger<UpdateCategoryCommand.UpdateCategoryCommandHandler>>();
    
    private readonly IFixture fixture = new Fixture();
    
    public UpdateCategoryCommandTests()
    {
        fixture.Customize<Category>(c => c.With(p => p.IsDeleted, false));
    }

    [Fact]
    public async Task ShouldReturnValidUpdateCategoryResponseModelWhenSuccessful()
    {
        var categories = fixture.Create<List<Category>>();
        repository.AllAsNoTracking().Returns(categories.BuildMock());

        var command = new UpdateCategoryCommand
        {
            Id = categories[0].Id,
            Name = fixture.Create<string>()
        };

        var handler = new UpdateCategoryCommand.UpdateCategoryCommandHandler(repository, logger);
        var result = await handler.Handle(command, new CancellationToken());
        var responseModel = result.AsT0;

        logger.Received(1).BeginScope(Arg.Any<Dictionary<string, object>>());
        result.IsT0.Should().BeTrue();
        responseModel.Name.Should().Be(command.Name);
        responseModel.Id.Should().Be(command.Id);
        repository.AllAsNoTracking().ToList()[0].Id.Should().Be(command.Id);
    }
    
    [Fact]
    public async Task ShouldReturnNotFoundWhenCategoryDoesntExist()
    {
        var categories = fixture.Create<List<Category>>();
        repository.AllAsNoTracking().Returns(categories.BuildMock());

        var command = new UpdateCategoryCommand
        {
            Id = categories[0].Id + 69,
            Name = fixture.Create<string>()
        };

        var handler = new UpdateCategoryCommand.UpdateCategoryCommandHandler(repository, logger);
        var result = await handler.Handle(command, new CancellationToken());
        
        result.IsT1.Should().BeTrue();
    }
    
    [Fact]
    public async Task ShouldReturnUniqueConstraintErrorWhenCategoryNameAlreadyExists()
    {
        var categories = fixture.Create<List<Category>>();
        repository.AllAsNoTracking().Returns(categories.BuildMock());

        var command = new UpdateCategoryCommand
        {
            Id = categories[0].Id,
            Name = categories[0].Name
        };

        var handler = new UpdateCategoryCommand.UpdateCategoryCommandHandler(repository, logger);
        var result = await handler.Handle(command, new CancellationToken());
        
        result.IsT2.Should().BeTrue();
    }
}
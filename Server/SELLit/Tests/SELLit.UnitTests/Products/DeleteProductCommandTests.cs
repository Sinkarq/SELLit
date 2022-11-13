using Microsoft.Extensions.Logging;
using SELLit.Server.Features.Products.Commands.Delete;
using SELLit.Server.Services.Interfaces;

namespace SELLit.UnitTests.Products;

public class DeleteProductCommandTests
{
    private readonly IDeletableEntityRepository<Product> repository 
        = Substitute.For<IDeletableEntityRepository<Product>>();
    private readonly ILogger<DeleteProductCommand.DeleteProductCommandHandler> logger
        = Substitute.For<ILogger<DeleteProductCommand.DeleteProductCommandHandler>>();
    private readonly IFixture fixture = new Fixture();

    public DeleteProductCommandTests()
    {
        var category = new Category
        {
            Id = 69,
            Name = fixture.Create<string>()
        };
        var userId = Guid.NewGuid().ToString();
        fixture.Customize<Product>(x => x.With(prop => prop.Category, category));
        fixture.Customize<Product>(x => x.With(prop => prop.CategoryId, category.Id));
        fixture.Customize<Product>(x => x.With(prop => prop.UserId, userId));
    }

    [Fact]
    public async Task ShouldReturnValidResponseModelWhenDeletedSuccessfully()
    {
        var products = fixture.Create<List<Product>>();
        repository.AllAsNoTracking().Returns(products.BuildMock());

        var command = new DeleteProductCommand
        {
            Id = products[0].Id
        };

        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.UserId.Returns(products[0].UserId);
        
        var handler = new DeleteProductCommand.DeleteProductCommandHandler(repository, currentUser, logger);
        var result = await handler.Handle(command, new CancellationToken());

        result.IsT0.Should().BeTrue();
    }
    
    [Fact]
    public async Task ShouldReturnNotFoundWhenProductDoesntExist()
    {
        var products = fixture.Create<List<Product>>();
        repository.AllAsNoTracking().Returns(products.BuildMock());

        var command = new DeleteProductCommand
        {
            Id = products[0].Id + 69
        };

        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.UserId.Returns(products[0].UserId);
        
        var handler = new DeleteProductCommand.DeleteProductCommandHandler(repository, currentUser, logger);
        var result = await handler.Handle(command, new CancellationToken());

        result.IsT1.Should().BeTrue();
    }
    
    [Fact]
    public async Task ShouldReturnForbiddenWhenProductIsNotOwnedByRequester()
    {
        var products = fixture.Create<List<Product>>();
        repository.AllAsNoTracking().Returns(products.BuildMock());

        var command = new DeleteProductCommand
        {
            Id = products[0].Id
        };

        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.UserId.Returns(Guid.NewGuid().ToString());
        
        var handler = new DeleteProductCommand.DeleteProductCommandHandler(repository, currentUser, logger);
        var result = await handler.Handle(command, new CancellationToken());

        result.IsT2.Should().BeTrue();
    }
}
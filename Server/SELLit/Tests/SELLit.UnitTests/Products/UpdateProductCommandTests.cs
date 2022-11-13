using AutoMapper;
using Microsoft.Extensions.Logging;
using SELLit.Server;
using SELLit.Server.Features.Products.Commands.Update;
using SELLit.Server.Infrastructure.Mapping;
using SELLit.Server.Services.Interfaces;

namespace SELLit.UnitTests.Products;

public class UpdateProductCommandTests
{
    private readonly IDeletableEntityRepository<Product> repository 
        = Substitute.For<IDeletableEntityRepository<Product>>();
    private readonly ILogger<UpdateProductCommand.UpdateProductCommandHandler> logger
        = Substitute.For<ILogger<UpdateProductCommand.UpdateProductCommandHandler>>();

    private readonly IMapper mapper;
    private readonly IFixture fixture = new Fixture();

    public UpdateProductCommandTests()
    {
        AutoMapperConfig.RegisterMappings(typeof(Startup).Assembly);
        this.mapper = AutoMapperConfig.MapperInstance;
        FixtureCustomization();
    }

    [Fact]
    public async Task ShouldReturnValidResponseModelWhenSuccessful()
    {
        var products = fixture.Create<List<Product>>();
        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.UserId.Returns(products[0].UserId);
        repository.AllAsNoTracking().Returns(products.BuildMock());
        var handler = new UpdateProductCommand.UpdateProductCommandHandler(repository, mapper, currentUser, logger);

        var command = fixture.Create<UpdateProductCommand>();
        command.Id = products[0].Id;

        var result = await handler.Handle(command, new CancellationToken());
        var responseModel = result.AsT0;
        
        result.IsT0.Should().BeTrue();
        responseModel.Should().NotBeNull();
    }
    
    [Fact]
    public async Task ShouldReturnNotFoundWhenProductDoesntExist()
    {
        var products = fixture.Create<List<Product>>();
        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.UserId.Returns(products[0].UserId);
        repository.AllAsNoTracking().Returns(products.BuildMock());
        var handler = new UpdateProductCommand.UpdateProductCommandHandler(repository, mapper, currentUser, logger);

        var command = fixture.Create<UpdateProductCommand>();
        command.Id = products[0].Id + 69;

        var result = await handler.Handle(command, new CancellationToken());
        
        result.IsT1.Should().BeTrue();
    }
    
    [Fact]
    public async Task ShouldReturnForbiddenWhenRequesterDoesntOwnTheProductEntity()
    {
        var products = fixture.Create<List<Product>>();
        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.UserId.Returns(Guid.NewGuid().ToString());
        repository.AllAsNoTracking().Returns(products.BuildMock());
        var handler = new UpdateProductCommand.UpdateProductCommandHandler(repository, mapper, currentUser, logger);

        var command = fixture.Create<UpdateProductCommand>();
        command.Id = products[0].Id;

        var result = await handler.Handle(command, new CancellationToken());
        
        result.IsT2.Should().BeTrue();
    }
    
    private void FixtureCustomization()
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
}
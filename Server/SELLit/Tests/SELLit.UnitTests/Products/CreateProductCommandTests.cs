using AutoMapper;
using Microsoft.Extensions.Logging;
using SELLit.Server;
using SELLit.Server.Features.Products.Commands.Create;
using SELLit.Server.Infrastructure.Mapping;
using SELLit.Server.Services.Interfaces;

namespace SELLit.UnitTests.Products;

public class CreateProductCommandTests
{
    private readonly IDeletableEntityRepository<Product> repository 
        = Substitute.For<IDeletableEntityRepository<Product>>();
    private readonly ILogger<CreateProductCommand.CreateProductCommandHandler> logger
        = Substitute.For<ILogger<CreateProductCommand.CreateProductCommandHandler>>();

    private readonly IMapper mapper;
    private readonly IFixture fixture = new Fixture();

    public CreateProductCommandTests()
    {
        AutoMapperConfig.RegisterMappings(typeof(Startup).Assembly);
        this.mapper = AutoMapperConfig.MapperInstance;
        FixtureCustomization();
    }

    [Fact]
    public async Task ShouldReturnValidCreateCategoryResponseModelWhenSuccessful()
    {
        repository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);
        var command = fixture.Create<CreateProductCommand>();
        var currentUser = Substitute.For<ICurrentUser>();
        var handler = new CreateProductCommand.CreateProductCommandHandler(repository, mapper, currentUser, logger);
        var result = await handler.Handle(command, new CancellationToken());

        result.Should().NotBeNull();
    }
    
    [Fact]
    public async Task ShouldReturnNullWhen()
    {
        repository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(0);
        var command = fixture.Create<CreateProductCommand>();
        var currentUser = Substitute.For<ICurrentUser>();
        var handler = new CreateProductCommand.CreateProductCommandHandler(repository, mapper, currentUser, logger);
        var result = await handler.Handle(command, new CancellationToken());

        result.Should().BeNull();
    }
    
    private void FixtureCustomization()
    {
        var category = new Category
        {
            Id = 69,
            Name = fixture.Create<string>()
        };
        var userId = Guid.NewGuid().ToString();
        fixture.Customize<CreateProductCommand>(x => x.With(prop => prop.CategoryId, category.Id));
        fixture.Customize<Product>(x => x.With(prop => prop.Category, category));
        fixture.Customize<Product>(x => x.With(prop => prop.CategoryId, category.Id));
        fixture.Customize<Product>(x => x.With(prop => prop.UserId, userId));
    }
}
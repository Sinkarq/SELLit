using SELLit.Server.Features.Categories.Commands.Create;
using SELLit.Server.Infrastructure;
using IHashids = HashidsNet.IHashids;

namespace SELLit.IntegrationTests.CategoriesController;

[Collection(nameof(IntegrationTests))]
public class UpdateCategoryTests
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;
    private readonly IHashids hashids;
    private readonly HttpClient httpClient;
    private const string UpdateCategoryRoute = Routes.Categories.Update;

    public UpdateCategoryTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory)
    {
        Factory = factory;
        this.httpClient = factory.HttpClient;
        this.hashids = factory.Hashids;
    }

    [Fact]
    public async Task Should_Succeed()
    {
        var id = this.hashids.Encode(2);
        var name = new Faker().Person.FirstName;

        await httpClient
            .WithAdminAuthentication()
            .PutAsJsonShouldBeWithStatusCodeAsync(UpdateCategoryRoute,
                new UpdateCategoryTestCommand()
                {
                    Id = 2,
                    Name = name
                }, HttpStatusCode.OK);

        (await httpClient
            .WithNoAuthentication()
            .DeserializeGetShouldBeWithStatusCodeAsync<GetCategoryQueryTestResponseModel>(
                Routes.Categories.GetById(id),
                HttpStatusCode.OK)).Name.Should().Be(name);
    }

    [Fact]
    public async Task Should_Return_NotFound() =>
        await httpClient
            .WithAdminAuthentication()
            .PutAsJsonShouldBeWithStatusCodeAsync(UpdateCategoryRoute,
                new UpdateCategoryTestCommand()
                {
                    Id = 69,
                    Name = new Faker().Person.FirstName
                }, HttpStatusCode.NotFound);

    [Fact]
    public async Task Should_Return_Unauthorized() =>
        await httpClient
            .WithNoAuthentication()
            .PutAsJsonShouldBeWithStatusCodeAsync(UpdateCategoryRoute,
                new UpdateCategoryTestCommand()
                {
                    Id = 1,
                    Name = new Faker().Person.FirstName
                }, HttpStatusCode.Unauthorized);

    [Fact]
    public async Task Should_Return_Forbidden() =>
        await httpClient
            .WithDefaultAuthentication()
            .PutAsJsonShouldBeWithStatusCodeAsync(UpdateCategoryRoute,
                new UpdateCategoryTestCommand()
                {
                    Id = 1,
                    Name = new Faker().Person.FirstName
                }, HttpStatusCode.Forbidden);

    [Fact]
    public async Task Should_Return_Bad_Request_UniqueName_Required()
    {
        var name = new Faker().Person.FirstName;

        await httpClient.WithAdminAuthentication().PostAsJsonShouldBeWithStatusCodeAsync(Routes.Categories.Create,
            new CreateCategoryCommand
            {
                Name = name
            }, HttpStatusCode.Created);
        
        await httpClient
            .WithAdminAuthentication()
            .DeserializePutAsJsonShouldBeWithStatusCodeAsync<ErrorResponse, UpdateCategoryTestCommand>(
                UpdateCategoryRoute,
                new UpdateCategoryTestCommand
                {
                    Id = Factory.ActiveCategories[0].Id,
                    Name = name
                }, HttpStatusCode.BadRequest);
    }
}

public sealed class UpdateCategoryTestCommand
{
    public int Id { get; set; } = default!;
    public string Name { get; set; } = default!;
}
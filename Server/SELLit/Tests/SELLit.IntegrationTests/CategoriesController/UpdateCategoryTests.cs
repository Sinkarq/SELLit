using Microsoft.Extensions.DependencyInjection;
using SELLit.Server.Infrastructure;

namespace SELLit.IntegrationTests.CategoriesController;

public class UpdateCategoryTests : IntegrationTestBase
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;
    private const string UpdateCategoryRoute = Routes.Categories.Update;

    public UpdateCategoryTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory) : base(factory)
    {
        Factory = factory;
    }

    [Fact]
    public async Task Should_Succeed()
    {
        var httpClient = Factory.CreateClient();
        var hashids = this.ServiceProvider.GetService<IHashids>();
        var id = hashids.Encode(1);
        var name = Guid.NewGuid().ToString();
        
        await AuthenticateAdminAsync(httpClient);
        
        await httpClient.PutAsJsonShouldBeWithStatusCodeAsync(UpdateCategoryRoute,
            new UpdateCategoryTestCommand()
            {
                Id = 1,
                Name = name
            }, HttpStatusCode.OK);

        (await httpClient.DeserializeGetShouldBeWithStatusCodeAsync<GetCategoryQueryTestResponseModel>(
            Routes.Categories.GetById(id), HttpStatusCode.OK)).Name.Should().Be(name);
    }   
    
    [Fact]
    public async Task Should_Return_NotFound()
    {
        var httpClient = Factory.CreateClient();
        var name = Guid.NewGuid().ToString();
        
        await AuthenticateAdminAsync(httpClient);
        
        await httpClient.PutAsJsonShouldBeWithStatusCodeAsync(UpdateCategoryRoute,
            new UpdateCategoryTestCommand()
            {
                Id = 69,
                Name = name
            }, HttpStatusCode.NotFound);
    }   
    
    [Fact]
    public async Task Should_Return_Unauthorized()
    {
        var httpClient = Factory.CreateClient();
        var name = Guid.NewGuid().ToString();

        await httpClient.PutAsJsonShouldBeWithStatusCodeAsync(UpdateCategoryRoute,
            new UpdateCategoryTestCommand()
            {
                Id = 1,
                Name = name
            }, HttpStatusCode.Unauthorized);
    }   
    
    [Fact]
    public async Task Should_Return_Forbidden()
    {
        var httpClient = Factory.CreateClient();

        await AuthenticateAsync(httpClient);
        
        var name = Guid.NewGuid().ToString();

        await httpClient.PutAsJsonShouldBeWithStatusCodeAsync(UpdateCategoryRoute,
            new UpdateCategoryTestCommand()
            {
                Id = 1,
                Name = name
            }, HttpStatusCode.Forbidden);
    }   
    
    [Fact]
    public async Task Should_Return_Bad_Request_UniqueName_Required()
    {
        var httpClient = Factory.CreateClient();
        
        await AuthenticateAdminAsync(httpClient);

        (await httpClient.DeserializePutAsJsonShouldBeWithStatusCodeAsync<ErrorModel, UpdateCategoryTestCommand>(UpdateCategoryRoute,
            new UpdateCategoryTestCommand
            {
                Id = 2,
                Name = "secondCategory"
            }, HttpStatusCode.BadRequest)).Errors.Should().NotBeNull().And.NotContainNulls();
    }
}

public sealed class UpdateCategoryTestCommand {
    public int Id { get; set; }
    public string Name { get; set; }
}
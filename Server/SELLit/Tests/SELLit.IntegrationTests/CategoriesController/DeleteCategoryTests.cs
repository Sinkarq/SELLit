using IHashids = HashidsNet.IHashids;

namespace SELLit.IntegrationTests.CategoriesController;

[Collection(nameof(IntegrationTests))]
public class DeleteCategoryTests
{
    private readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;
    private readonly IHashids hashids;
    private readonly HttpClient httpClient;

    public DeleteCategoryTests(IntegrationTestFactory<Startup, ApplicationDbContext> factory)
    {
        Factory = factory;
        this.httpClient = factory.HttpClient;
        this.hashids = factory.Hashids;
    }

    [Fact]
    public async Task Should_Delete_And_Return_OK()
    {
        var id = this.hashids.Encode(Factory.ActiveCategories[0].Id);

        await httpClient
            .WithAdminAuthentication()
            .DeleteShouldBeWithStatusCodeAsync(Routes.Categories.DeleteById(id), HttpStatusCode.OK);

        await httpClient.GetShouldBeWithStatusCodeAsync(Routes.Categories.GetById(id), HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_Return_NotFound() =>
        await httpClient
            .WithAdminAuthentication()
            .DeleteShouldBeWithStatusCodeAsync(
                Routes.Categories.DeleteById(hashids.Encode(69)),
                HttpStatusCode.NotFound);

    [Fact]
    public async Task Should_Return_Unauthorized_When_Not_LoggedIn() =>
        await httpClient
            .WithNoAuthentication()
            .DeleteShouldBeWithStatusCodeAsync(
                Routes.Categories.DeleteById(this.hashids.Encode(69)),
                HttpStatusCode.Unauthorized);

    [Fact]
    public async Task Should_Return_Forbidden() =>
        await httpClient
            .WithDefaultAuthentication()
            .DeleteShouldBeWithStatusCodeAsync(
                Routes.Categories.DeleteById(this.hashids.Encode(69)),
                HttpStatusCode.Forbidden);
}
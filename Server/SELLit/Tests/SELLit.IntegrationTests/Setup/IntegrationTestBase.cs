using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SELLit.Common;
using SELLit.Data;
using SELLit.Data.Models;
using SELLit.Data.Seeding;
using SELLit.Server;
using SELLit.Server.Features;
using SELLit.Server.Features.Identity.Commands.Login;

namespace SELLit.IntegrationTests.Setup;

public class IntegrationTestBase : IClassFixture<IntegrationTestFactory<Startup, ApplicationDbContext>>
{
    public readonly IntegrationTestFactory<Startup, ApplicationDbContext> Factory;
    public readonly ApplicationDbContext DbContext;
    public readonly IServiceProvider ServiceProvider;

    public IntegrationTestBase(IntegrationTestFactory<Startup, ApplicationDbContext> factory)
    {
        Factory = factory;
        var scope = factory.Services.CreateScope();
        DbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        this.ServiceProvider = scope.ServiceProvider;
        new ApplicationDbContextSeeder().SeedAsync(DbContext, scope.ServiceProvider).GetAwaiter().GetResult();
        SeedDatabase(DbContext).GetAwaiter().GetResult();
    }

    private async Task SeedDatabase(ApplicationDbContext dbContext)
    {
        var categories = new List<Category>()
        {
            new Category("firstCategory"),
            new Category("secondCategory")
        };

        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();

        var users = await dbContext.Users.ToListAsync();
        var user = users[1];

        var products = new List<Product>()
        {
            new Product("title", "desc", "loc", "phoneNumber", 69.420, categories[0].Id, user.Id,
                DeliveryResponsibility.Buyer),
            new Product("title", "desc", "loc", "phoneNumber", 69.420, categories[0].Id, user.Id,
                DeliveryResponsibility.Buyer),
        };
        await dbContext.Products.AddRangeAsync(products);
        await dbContext.SaveChangesAsync();
    }
    
    protected static async Task AuthenticateAdminAsync(HttpClient client)
    {
        var response = await client.PostAsJsonAsync(Routes.Identity.Login, new LoginCommand()
        {
            Username = "Sinkarq",
            Password = "password1234"
        }).DeserializeHttpContentAsync<LoginCommandResponseModel>();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", response.Token);
    }
    
    protected static async Task AuthenticateAsync(HttpClient client)
    {
        var response = await client.PostAsJsonAsync(Routes.Identity.Login, new LoginCommand()
        {
            Username = "John",
            Password = "password1234"
        }).DeserializeHttpContentAsync<LoginCommandResponseModel>();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", response.Token);
    }
}
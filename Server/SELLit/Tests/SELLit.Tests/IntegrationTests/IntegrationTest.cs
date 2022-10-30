using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SELLit.Common;
using SELLit.Data;
using SELLit.Data.Models;
using SELLit.Server;
using SELLit.Server.Features;
using SELLit.Server.Features.Identity.Commands.Login;

namespace SELLit.Tests.IntegrationTests;

public class IntegrationTest
{
    protected readonly HttpClient HttpClient;
    protected ServiceProvider ServiceProvider;
    
    protected IntegrationTest()
    {
        var appFactory = new WebApplicationFactory<Startup>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                             typeof(DbContextOptions<ApplicationDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }
                    services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("TestDb"));
                    var sp = services.BuildServiceProvider();
                    this.ServiceProvider = sp;
                });
            });

        this.HttpClient = appFactory.CreateClient();
        var db = this.ServiceProvider.GetService<ApplicationDbContext>();
        SeedDatabase(db).GetAwaiter().GetResult();
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

    protected async Task AuthenticateAdminAsync()
    {
        var response = await this.HttpClient.PostAsJsonAsync(Routes.Identity.Login, new LoginCommand()
        {
            Username = "Sinkarq",
            Password = "password1234"
        }).DeserializeHttpContentAsync<LoginCommandResponseModel>();

        this.HttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", response.Token);
    }
    
    protected async Task AuthenticateAsync()
    {
        var response = await this.HttpClient.PostAsJsonAsync(Routes.Identity.Login, new LoginCommand()
        {
            Username = "John",
            Password = "password1234"
        }).DeserializeHttpContentAsync<LoginCommandResponseModel>();

        this.HttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", response.Token);
    }
}
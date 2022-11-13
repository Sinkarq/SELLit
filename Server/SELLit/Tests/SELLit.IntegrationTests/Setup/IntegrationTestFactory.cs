using CommunityToolkit.Diagnostics;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SELLit.Data.Models;
using IHashids = HashidsNet.IHashids;

namespace SELLit.IntegrationTests.Setup;

public class IntegrationTestFactory<TProgram, TDbContext> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class 
    where TDbContext : DbContext
{
    private readonly TestcontainerDatabase container;
    public HttpClient HttpClient = default!;
    public IHashids Hashids = default!;
    private ApplicationDbContext dbContext = default!;

    public IntegrationTestFactory() =>
        container = new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(new MsSqlTestcontainerConfiguration
            {
                Password = "localdevpassword#123",
            })
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithCleanUp(true)
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveDbContext<ApplicationDbContext>();
            services.AddDbContext<TDbContext>(options => { options.UseSqlServer(container.ConnectionString + "TrustServerCertificate=True;"); });
            //services.AddTransient<ArtworkCreator>();
        });
    }

    public async Task InitializeAsync()
    {
        await container.StartAsync();
        this.HttpClient = CreateClient();
        var serviceProvider = this.Services.CreateScope().ServiceProvider;
        var context = serviceProvider.GetService<ApplicationDbContext>();
        Guard.IsNotNull(context, "ApplicationDbContext can't be resolved from the DI container");
        this.dbContext = context;
        context.SeedTestDatabaseAsync(serviceProvider).GetAwaiter().GetResult();
        this.Hashids = serviceProvider.GetService<IHashids>()!;
    }

    public List<Category> ActiveCategories => this.dbContext.Categories.ToList();

    public List<Product> ActiveProducts => this.dbContext.Products.ToList();

    public new async Task DisposeAsync() => await container.DisposeAsync();
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SELLit.Data.Models;
using SELLit.Data.Seeding;

namespace SELLit.IntegrationTests.Setup;

internal static class ExtensionMethods
{
    internal static void RemoveDbContext<T>(this IServiceCollection services) where T : DbContext
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<T>));
        if (descriptor != null) services.Remove(descriptor);
    }

    internal static void EnsureDbCreated<T>(this IServiceCollection services) where T : DbContext
    {
        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<T>();
        context.Database.EnsureCreated();
    }

    internal static async Task SeedTestDatabaseAsync(this ApplicationDbContext dbContext, IServiceProvider provider)
    { 
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Database.MigrateAsync();
        await new ApplicationDbContextSeeder().SeedAsync(dbContext, provider);
        var users = await dbContext.Users.ToListAsync();
        var categoriesSeeded = await SeedCategoriesAsync(dbContext);
        var productsSeeded = await SeedProductsAsync(dbContext, users[0].Id, categoriesSeeded[0].Id);
    }

    internal static async Task<List<Category>> SeedCategoriesAsync(this ApplicationDbContext dbContext)
    {
        var categories = new List<Category>()
        {
            new () {Name = "First"},
            new() {Name = "Second"},
            new() {Name = "Third"}
        };

        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();

        return categories;
    }

    internal static async Task<List<Product>> SeedProductsAsync(this ApplicationDbContext dbContext, string userId, int categoryId)
    {
        var products = CreateMockedProducts(userId, categoryId);

        await dbContext.AddRangeAsync(products);
        await dbContext.SaveChangesAsync();

        return products;
    }

    private static List<Product> CreateMockedProducts(string userId, int categoryId)
    {
        var products = new List<Product>()
        {
            new()
            {
                Title = "First",
                Description = "First",
                Location = "First",
                PhoneNumber = "First",
                Price = 69.420,
                OrderCount = 0,
                DeliveryResponsibility = DeliveryResponsibility.Buyer,
                UserId = userId,
                CategoryId = categoryId
            },
            new()
            {
                Title = "Second",
                Description = "First",
                Location = "First",
                PhoneNumber = "First",
                Price = 69.420,
                OrderCount = 0,
                DeliveryResponsibility = DeliveryResponsibility.Buyer,
                UserId = userId,
                CategoryId = categoryId
            },
            new()
            {
                Title = "Third",
                Description = "First",
                Location = "First",
                PhoneNumber = "First",
                Price = 69.420,
                OrderCount = 0,
                DeliveryResponsibility = DeliveryResponsibility.Buyer,
                UserId = userId,
                CategoryId = categoryId
            },
        };
        return products;
    }
}
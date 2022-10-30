using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SELLit.Data;
using SELLit.Data.Seeding;

namespace SELLit.Server.Infrastructure.Extensions;

internal static class ApplicationBuilderExtensions
{   
    public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder app)
    {
        return app.UseSwagger()
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "SELLit V1");
                options.RoutePrefix = string.Empty;
            });
    }
    
    public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!dbContext.Database.IsInMemory())
        {
            dbContext.Database.Migrate();
        }
        new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();

        return app;
    }
    
    private static bool IsInMemory(this DatabaseFacade database)
    {
        return database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory";
    }
    
}

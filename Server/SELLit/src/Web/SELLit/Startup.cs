using MediatR;
using Microsoft.EntityFrameworkCore;
using SELLit.Data;
using SELLit.Server.Infrastructure.Extensions;
using SELLit.Server.Infrastructure.Filters;

namespace SELLit.Server;

public class Startup
{
    public Startup(IConfiguration configuration)
        => this.Configuration = configuration;

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddDbContext<ApplicationDbContext>(
                options => { options.UseSqlServer(this.Configuration.GetDefaultConnection()); })
            .AddIdentity()
            .AddJwtAuthentication(services.GetApplicationSettings(this.Configuration))
            .AddApplicationServices()
            .AddMediatR(new[] {typeof(Startup).Assembly})
            .AddHttpContextAccessor()
            .AddSwagger()
            .AddDatabaseDeveloperPageExceptionFilter()
            .AddControllers(
                options =>
                {
                    options.Filters.Add<ModelOrNotFoundActionFilter>();
                });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }

        app
            .UseSwaggerUI()
            .SeedDatabase()
            .UseRouting()
            .UseCors(
                options => options
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod())
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
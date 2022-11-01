using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SELLit.Data;
using SELLit.Server.Infrastructure.Extensions;
using SELLit.Server.Infrastructure.Filters;

namespace SELLit.Server;

public class Startup
{
    private IWebHostEnvironment CurrentEnvironment { get; }
    
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        this.Configuration = configuration;
        this.CurrentEnvironment = env;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        ;
        services
            .AddDbContext<ApplicationDbContext>(
                options =>
                {
                    options.UseSqlServer(this.Configuration.GetDefaultConnection(), options =>
                    {
                        options.EnableRetryOnFailure(maxRetryCount: 4, TimeSpan.FromSeconds(1),
                            errorNumbersToAdd: new int[] { });
                    });

                    if (this.CurrentEnvironment.IsDevelopment())
                    {
                        options.EnableDetailedErrors();
                        options.EnableSensitiveDataLogging();
                        options.ConfigureWarnings(warningsAction =>
                        {
                            // TODO: Check these
                            warningsAction.Log(
                                CoreEventId.FirstWithoutOrderByAndFilterWarning,
                                CoreEventId.RowLimitingOperationWithoutOrderByWarning);
                        });
                    }
                })
            .AddIdentity()
            .AddJwtAuthentication(services.GetApplicationSettings(this.Configuration))
            .AddApplicationServices()
            .AddMediatR(typeof(Startup).Assembly)
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
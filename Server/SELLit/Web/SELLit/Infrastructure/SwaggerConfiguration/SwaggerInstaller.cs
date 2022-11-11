using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace SELLit.Server.Infrastructure.SwaggerConfiguration;

internal static class SwaggerInstaller
{
    internal static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder app) =>
        app
            .UseSwagger(options => options.RouteTemplate = "docs/{documentName}/docs.json")
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/docs/v1/docs.json", "SELLit V1");
                options.RoutePrefix = string.Empty;
                options.InjectStylesheet("/swagger-ui/SwaggerDark.css");
            });
    
    internal static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SELLit API",
                Version = "v1"
            });
            
            c.ExampleFilters();

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = "JWT Authorization header using bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
            
            c.EnableAnnotations();
        });

        services.AddSwaggerExamplesFromAssemblyOf<Startup>();

        return services;
    }
}
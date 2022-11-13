using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SELLit.Common;
using SELLit.Data;
using SELLit.Data.Common.Repositories;
using SELLit.Data.Repositories;
using SELLit.Server.Features.Identity;
using SELLit.Server.Features.Identity.Commands.Login;
using SELLit.Server.Infrastructure.Mapping;
using SELLit.Server.Services;
using SELLit.Server.Services.Interfaces;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace SELLit.Server.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        AppSettings appSettings)
    {
        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        => services
            .AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>))
            .AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
            .AddSingleton(Log.Logger)
            .AddAutoMapper()
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<LoginCommandValidator>()
            .AddHashids(x =>
            {
                x.MinHashLength = 8;
                x.Salt = "Sinkarq";
            })
            .AddTransient<IIdentityService, IdentityService>()
            .AddScoped<ICurrentUser, CurrentUser>();

    

    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        AutoMapperConfig.RegisterMappings(typeof(Startup).Assembly);
        
        services.AddSingleton(AutoMapperConfig.MapperInstance);
        
        return services;
    }

    
}
using SELLit.Common;

namespace SELLit.Server.Infrastructure.Extensions;

public static class ConfigurationSettings
{
    public static string GetDefaultConnection(this IConfiguration configuration)
    {
        var serverName = configuration["ServerName"];
        var database = configuration["Database"];
        var username = configuration["UserName"];
        var password = configuration["Password"];
        const string port = "1433";
        return $"Server={serverName},{port};Initial Catalog={database};User ID={username};Password={password}";
    }

    public static AppSettings GetApplicationSettings(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var applicationSettingsConfiguration = configuration.GetSection("ApplicationSettings");
        services.Configure<AppSettings>(applicationSettingsConfiguration);

        return applicationSettingsConfiguration.Get<AppSettings>();
    }
}
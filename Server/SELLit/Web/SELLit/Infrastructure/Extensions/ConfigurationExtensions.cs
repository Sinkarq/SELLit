using SELLit.Common;

namespace SELLit.Server.Infrastructure.Extensions;

public static class ConfigurationSettings
{
    public static string GetDefaultConnection(this IConfiguration configuration)
        => configuration.GetConnectionString("DefaultConnection");

    public static AppSettings GetApplicationSettings(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var applicationSettingsConfiguration = configuration.GetSection("ApplicationSettings");
        services.Configure<AppSettings>(applicationSettingsConfiguration);

        return applicationSettingsConfiguration.Get<AppSettings>();
    }
}
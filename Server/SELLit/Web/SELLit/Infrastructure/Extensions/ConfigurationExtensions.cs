using SELLit.Common;

namespace SELLit.Server.Infrastructure.Extensions;

internal static class ConfigurationSettings
{
    internal static string GetDefaultConnection(this IConfiguration configuration) 
        => configuration.GetConnectionString("DefaultConnection");

    internal static AppSettings GetApplicationSettings(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var applicationSettingsConfiguration = configuration.GetSection("ApplicationSettings");
        services.Configure<AppSettings>(applicationSettingsConfiguration);

        return applicationSettingsConfiguration.Get<AppSettings>();
    }
}
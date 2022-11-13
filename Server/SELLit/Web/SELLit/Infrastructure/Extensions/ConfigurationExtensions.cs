using SELLit.Common;

namespace SELLit.Server.Infrastructure.Extensions;

internal static class ConfigurationSettings
{
    internal static string GetDefaultConnection(this IConfiguration configuration) =>
        configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "Missing SQL Server connection string."
            + " "
            + "Please specify it under the DefaultConnection:App Config section.");

    internal static AppSettings GetApplicationSettings(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var applicationSettingsConfiguration = configuration.GetSection("ApplicationSettings");
        services.Configure<AppSettings>(applicationSettingsConfiguration);

        return applicationSettingsConfiguration.Get<AppSettings>()
            ?? throw new InvalidOperationException("Appsettings can't be binded to model");
    }
}
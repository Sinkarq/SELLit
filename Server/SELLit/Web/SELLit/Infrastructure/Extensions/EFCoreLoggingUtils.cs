namespace SELLit.Server.Infrastructure.Extensions;

public static class EFCoreLoggingUtils
{
    public static IDisposable EFQueryScope<T>(this ILogger<T> logger, string queryScopeName)
        => logger.BeginScope(new Dictionary<string, object>() { {"EFQueries", queryScopeName} });
    
    public static IDisposable EFQueryScope(this ILogger logger, string queryScopeName)
        => logger.BeginScope(new Dictionary<string, object>() { {"EFQueries", queryScopeName} });
}
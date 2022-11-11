using System.Diagnostics;
using Serilog;

namespace SELLit.Server;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(
                webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .CaptureStartupErrors(true)
                        .ConfigureAppConfiguration(config =>
                        {
                            config.AddJsonFile("appsettings.json", optional: true);
                        });
                })
            .UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name!)
                    .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment!);

                loggerConfiguration.Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached);
            });
}
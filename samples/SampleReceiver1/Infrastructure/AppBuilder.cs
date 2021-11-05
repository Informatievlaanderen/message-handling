

namespace SampleReceiver1.Infrastructure
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Serilog;
    public static class AppBuilder
    {
        public static TApp Build<TApp>(params string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .CreateLogger();

            var loggingFactory = LoggerFactory.Create(builder => builder.AddSerilog(dispose: true));

            // Build configuration
            Configuration.Initialize<Startup>();

            var serviceCollection = new ServiceCollection()
                .AddSingleton(loggingFactory)
                .AddSingleton(Configuration.Root)
                .AddTransient<App>();

            var startup = new Startup(Configuration.Root, args);
            startup.ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            try
            {
                return serviceProvider.GetService<TApp>()!;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error running service");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}

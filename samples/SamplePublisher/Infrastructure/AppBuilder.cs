using System;
using System.IO;
using Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace SamplePublisher.Infrastructure
{
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

namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple.Lambda
{
    using Microsoft.Extensions.Configuration;
    using System.IO;

    public class ConfigureService : IConfigureService
    {
        public IEnvironmentService EnvironmentService { get; }

        public ConfigureService(IEnvironmentService environmentService)
        {
            EnvironmentService = environmentService;
        }

        public IConfiguration Configuration => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{EnvironmentService.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }
}

using System.Linq;

namespace SamplePublisher.Infrastructure
{
    using System.IO;
    using Microsoft.Extensions.Configuration;

    internal static class Configuration
    {
        public static IConfigurationRoot Root { get; private set; }

        public static IConfigurationRoot Initialize<TStartup>()
        {
            string basePath = Path.GetDirectoryName(typeof(TStartup).Assembly.Location)!;
            Root = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFiles(basePath)
                .Build();
            return Root;
        }

        private static IConfigurationBuilder AddJsonFiles(this IConfigurationBuilder builder, string basePath)
        {
            if (string.IsNullOrWhiteSpace(basePath))
                return builder;

            var defaultAppSettings = Directory.EnumerateFiles(basePath,
                "appsettings.json",
                SearchOption.AllDirectories)
                .FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(defaultAppSettings))
            {
                defaultAppSettings = defaultAppSettings.Replace(basePath, "");
                defaultAppSettings = defaultAppSettings.StartsWith('/') ? defaultAppSettings.Remove(0, 1) : defaultAppSettings;

                builder.AddJsonFile(defaultAppSettings, optional: false, reloadOnChange: true);
            }

            return builder;
        }
    }
}

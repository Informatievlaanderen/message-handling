

namespace SampleKafkaPublisher.Infrastructure
{
    using Be.Vlaanderen.Basisregisters.MessageHandling.Kafka;
    using Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Configurations;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        private IConfigurationRoot _configuration;

        public Startup(IConfigurationRoot configuration, params string[] args)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging()
                .AddMessageHandling(_configuration.GetSection("KafkaSettings").Get<KafkaConfiguration>())
                .AddScoped<MunicipalityEventProducer>();
        }

    }
}

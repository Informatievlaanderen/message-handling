

namespace SamplePublisher.Infrastructure
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq;
    using Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq.Configurations;
    using Publishers;

    public class Startup
    {
        private IConfigurationRoot _configuration;

        public Startup(IConfigurationRoot configuration, params string[] args)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging();
            serviceCollection.AddMessageHandling(_configuration.GetSection("MessageHandling").Get<MessageHandlerConfiguration>());
            serviceCollection.AddScoped<MessagePublisher>();
            serviceCollection.AddScoped<BarMessagePublisher>();
            serviceCollection.AddScoped<StreetNameMessagePublisher>();
            serviceCollection.AddScoped<AddressMessagePublisher>();
        }

    }
}

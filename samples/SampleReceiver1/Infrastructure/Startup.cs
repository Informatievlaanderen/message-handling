using Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq.Configurations;

namespace SampleReceiver1.Infrastructure
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq;
    using Consumers;

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
            serviceCollection.AddScoped<MessageConsumer>();
            serviceCollection.AddScoped<DirectMessageConsumer>();
        }
    }
}

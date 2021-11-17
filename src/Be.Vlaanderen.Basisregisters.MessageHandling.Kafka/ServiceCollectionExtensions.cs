namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    using Configurations;
    using Confluent.Kafka;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageHandling(this IServiceCollection serviceCollection, KafkaConfiguration config)
        {
            var context = new KafkaContext(
                new ClientConfig(config.ClientConfig),
                config.ProduceTopics,
                new Environment(config.Environment),
                new ProducerName(config.ProducerName));

            serviceCollection.AddSingleton(context);
            return serviceCollection;
        }
    }
}

namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    using System.Collections.Generic;
    using System.Linq;
    using Configurations;
    using Microsoft.Extensions.DependencyInjection;
    using RabbitMQ.Client;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageHandling(this IServiceCollection serviceCollection, MessageHandlerConfiguration config)
        {
            var connectionFactory = new ConnectionFactory()
            {
                Uri = config.RabbitMqConfig.Uri,
                UserName = config.RabbitMqConfig.Username,
                Password = config.RabbitMqConfig.Password,
                Port = config.RabbitMqConfig.Port
            };
            var connection = connectionFactory.CreateConnection();
            var modules = config.OtherModules?.Select(m => new Module(m)).ToList() ?? new List<Module>();
            var module = new Module(config.ThisModule);

            var context = new MessageHandlerContext(
                connection,
                new Environment(config.Environment),
                modules,
                module);

            serviceCollection.AddSingleton(context);
            return serviceCollection;
        }
    }
}

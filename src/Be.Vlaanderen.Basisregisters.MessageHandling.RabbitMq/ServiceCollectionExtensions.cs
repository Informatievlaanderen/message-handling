namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using Configurations;
    using Microsoft.Extensions.DependencyInjection;
    using RabbitMQ.Client;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageHandling(this IServiceCollection serviceCollection, MessageHandlerConfiguration config)
        {
            var connectionFactory = new ConnectionFactory()
            {
                Uri = config.RabbitMq.Uri,
                UserName = config.RabbitMq.Username,
                Password = config.RabbitMq.Password,
                Port = config.RabbitMq.Port
            };
            var connection = connectionFactory.CreateConnection();
            var modules = config.Modules?.Select(m => new Module(m)).ToList() ?? new List<Module>();
            var module = new Module(config.Module);

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

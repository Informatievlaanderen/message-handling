using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple.Lambda
{
    using System;
    using System.Threading.Tasks;
    using Amazon.Lambda.SQSEvents;
    using Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

    public class SqsLambdaFunction
    {
        private readonly ServiceProvider _serviceProvider;

        public IConfigureService ConfigureService { get; set; }

        public SqsLambdaFunction()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            ConfigureService = _serviceProvider.GetRequiredService<IConfigureService>();
        }

        public async Task HandleSqsEvent(SQSEvent sqsEvent, ILambdaContext context)
        {
            var serializer = JsonSerializer.CreateDefault();

            foreach (var record in sqsEvent.Records)
            {
                _ = record.MessageId;
                _ = record.EventSource;

                var json = record.Body;
                var sqsJsonMessage = serializer.Deserialize<SqsJsonMessage>(json);
                if (sqsJsonMessage is not null)
                {
                    await ProcessMessage(sqsJsonMessage);
                }
            }
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient<IEnvironmentService, EnvironmentService>();
            services.AddTransient<IConfigureService, ConfigureService>();
            services.AddTransient<IMessageHandler, MessageHandler>();
        }

        private async Task ProcessMessage(SqsJsonMessage sqsJsonMessage)
        {
            await Task.Yield();
            var messageData = sqsJsonMessage.Map() ?? throw new ArgumentException("SQS message data is null.");

            var messageHandler = _serviceProvider.GetRequiredService<IMessageHandler>();
            await messageHandler.HandleMessage(messageData);
        }
    }
}

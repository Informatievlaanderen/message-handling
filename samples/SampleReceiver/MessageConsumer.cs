using System;
using System.Text.Json;
using RabbitMQ.Client;
using Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq;
using Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq.Definitions;
using Environment = Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq.Environment;

namespace SampleReceiver
{
    public class MessageConsumer : Consumer<Message>
    {
        public MessageConsumer(IConnection connection) : base(
            new QueueDefinition(
                MessageType.Direct,
                Environment.Development,
                Module.StreetName,
                new QueueName("root")), connection)
        {
        }

        protected override Message Parse(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return null!;
            return JsonSerializer.Deserialize<Message>(message)!;
        }

        protected override void MessageReceive(Message message, ulong deliveryTag)
        {
            Console.WriteLine(message.ToString());

            Ack(deliveryTag);
        }

        protected override void MessageReceiveException(Exception exception, ulong deliveryTag)
        {
            Reject(deliveryTag);
        }
    }
}

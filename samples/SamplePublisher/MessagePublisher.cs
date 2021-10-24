using System;
using RabbitMQ.Client;
using Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq;
using Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq.Definitions;
using Environment = Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq.Environment;

namespace SamplePublisher
{
    public class MessagePublisher : Producer<Message>
    {
        public MessagePublisher(IConnection connection) : base(
            new RouteDefinition(
                MessageType.Direct,
                Environment.Development,
                Module.StreetName,
                "root"), connection) { }

        protected override void OnPublishMessagesHandler(Message[] messages)
        {
            Console.WriteLine("Message send :)");
        }

        protected override void OnPublishMessagesExceptionHandler(Exception exception, Message[] messages)
        {
            Console.WriteLine(exception.Message);
        }
    }
}

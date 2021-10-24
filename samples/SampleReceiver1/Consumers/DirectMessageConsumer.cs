namespace SampleReceiver1.Consumers
{
    using System;
    using System.Text.Json;
    using Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq;
    using Messages;

    public class DirectMessageConsumer : DirectConsumer<Message>
    {
        public DirectMessageConsumer(MessageHandlerContext context) : base(
                context,
                new Module("municipality-registry"), "events")
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
            Console.WriteLine("RECEIVING DIRECT MESSAGE FROM: municipality-registry");
            Console.WriteLine(message.ToString());

            Ack(deliveryTag);
        }

        protected override void MessageReceiveException(Exception exception, ulong deliveryTag)
        {
            Reject(deliveryTag);
        }
    }
}

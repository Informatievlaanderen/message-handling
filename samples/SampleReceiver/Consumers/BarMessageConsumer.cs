namespace SampleReceiver.Consumers
{
    using System;
    using System.Text.Json;
    using Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq;
    using Messages;

    public class BarMessageConsumer : TopicConsumer<Bar>
    {
        public BarMessageConsumer(MessageHandlerContext context) : base(
                context,
                new Module("municipality-registry"), "events")
        {
        }

        protected override Bar Parse(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return null!;
            return JsonSerializer.Deserialize<Bar>(message)!;
        }

        protected override void MessageReceive(Bar message, ulong deliveryTag)
        {
            Console.WriteLine("RECEIVING BAR MESSAGE FROM: municipality-registry");
            Console.WriteLine(message.ToString());
            Ack(deliveryTag);
        }

        protected override void MessageReceiveException(Exception exception, ulong deliveryTag)
        {
            Reject(deliveryTag);
        }
    }
}

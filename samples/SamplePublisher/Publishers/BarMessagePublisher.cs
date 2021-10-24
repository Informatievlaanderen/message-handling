namespace SamplePublisher.Publishers
{
    using System;
    using Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq;
    using Messages;

    public class BarMessagePublisher : TopicProducer<Bar>
    {
        public BarMessagePublisher(MessageHandlerContext context) : base(context, "events") { }

        protected override void OnPublishMessagesHandler(Bar[] messages)
        {
            Console.WriteLine("Message send :)");
        }

        protected override void OnPublishMessagesExceptionHandler(Exception exception, Bar[] messages)
        {
            Console.WriteLine(exception.Message);
        }
    }
}

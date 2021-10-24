namespace SamplePublisher.Publishers
{
    using System;
    using Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq;
    using Messages;

    public class MessagePublisher : TopicProducer<Message>
    {
        public MessagePublisher(MessageHandlerContext context) : base(context, "events") { }

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

namespace SamplePublisher.Publishers
{
    using System;
    using Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq;
    using Messages;

    public class StreetNameMessagePublisher : DirectProducer<Message>
    {
        public StreetNameMessagePublisher(MessageHandlerContext context) : base(context, new Module("streetname-registry"), "events") { }

        protected override void OnPublishMessagesHandler(Message[] messages)
        {
            Console.WriteLine("Direct message send to streetname-registry :)");
        }

        protected override void OnPublishMessagesExceptionHandler(Exception exception, Message[] messages)
        {
            Console.WriteLine(exception.Message);
        }
    }
}

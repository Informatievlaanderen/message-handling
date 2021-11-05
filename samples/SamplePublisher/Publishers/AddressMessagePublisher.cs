namespace SamplePublisher.Publishers
{
    using System;
    using Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq;
    using Messages;

    public class AddressMessagePublisher : DirectProducer<Message>
    {
        public AddressMessagePublisher(MessageHandlerContext context) : base(context, new Module("address-registry")) { }

        protected override void OnPublishMessagesHandler(Message[] messages)
        {
            Console.WriteLine("Direct message send to address-registry :)");
        }

        protected override void OnPublishMessagesExceptionHandler(Exception exception, Message[] messages)
        {
            Console.WriteLine(exception.Message);
        }
    }
}

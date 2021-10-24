namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq.Definitions
{
    using System;

    public sealed class RouteDefinition
    {
        public RabbitMq.Environment Environment { get; }
        public MessageType MessageType { get; }
        public Module Module { get; }
        public RouteKey RouteKey { get; }
        public Exchange Exchange { get; }

        public RouteDefinition(MessageHandlerContext context, MessageType messageType, string subscriber, string queueName)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            Environment = context.Environment;
            MessageType = messageType;
            Module = context.ThisModule;
            RouteKey = RouteKey.Create(messageType, context.Environment, context.ThisModule, subscriber, queueName);
            Exchange = Exchange.Create(messageType, context.Environment, context.ThisModule);
        }
    }
}

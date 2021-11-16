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

        /// <summary>
        /// The ctor for the route definition
        /// </summary>
        /// <param name="context">The message handler context</param>
        /// <param name="messageType">The message type (direct, topic)</param>
        /// <param name="subscriber">The subscriber</param>
        /// <param name="queueName">The queue name</param>
        public RouteDefinition(
            MessageHandlerContext context,
            MessageType messageType,
            string subscriber,
            string queueName)
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

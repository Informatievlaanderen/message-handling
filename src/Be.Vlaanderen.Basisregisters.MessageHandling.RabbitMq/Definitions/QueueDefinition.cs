namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq.Definitions
{
    using System;
    using System.Collections.Generic;
    using Environment = RabbitMq.Environment;

    public sealed class QueueDefinition
    {
        public Environment Environment { get; }
        public MessageType MessageType { get; }
        public Module Module { get; }
        public RouteKey RouteKey { get; }
        public Exchange Exchange { get; }
        public RouteKey BindingKey { get; }
        public string FullQueueName => RouteKey.Value;
        public string QueueName { get; }
        public string DlxName { get; }

        public QueueDefinition(MessageHandlerContext context, MessageType messageType, Module publisher, string queueName)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if(!context.OtherModules.Contains(publisher))
                throw new ArgumentException($"Module '{publisher}' is not registered");

            Environment = context.Environment;
            MessageType = messageType;
            Module = context.ThisModule;
            QueueName = queueName;
            RouteKey = RouteKey.Create(messageType, context.Environment, publisher, Module, QueueName);
            Exchange = Exchange.Create(messageType, context.Environment, publisher);
            DlxName = $"dlx.{messageType}.{context.Environment}.{publisher}";

            if (messageType == MessageType.Direct)
                BindingKey = RouteKey;
            if (messageType == MessageType.Topic)
                BindingKey = RouteKey.Create(messageType, context.Environment, publisher, "*", queueName);
        }
    }
}

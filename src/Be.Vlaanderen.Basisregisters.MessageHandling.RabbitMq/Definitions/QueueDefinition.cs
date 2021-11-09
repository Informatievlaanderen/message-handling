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
        public Module QueueName { get; }
        public string DlxName { get; }

        public QueueDefinition(MessageHandlerContext context, MessageType messageType, Module module)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if(!context.OtherModules.Contains(module))
                throw new ArgumentException($"Module '{module}' is not registered");

            Environment = context.Environment;
            MessageType = messageType;
            Module = context.ThisModule;
            RouteKey = RouteKey.Create(messageType, context.Environment, module, context.ThisModule);
            Exchange = Exchange.Create(messageType, context.Environment, module);
            QueueName = context.ThisModule;
            DlxName = $"dlx.{messageType}.{context.Environment}.{module}";

            if (messageType == MessageType.Direct)
                BindingKey = RouteKey;
            if (messageType == MessageType.Topic)
                BindingKey = RouteKey.Create(messageType, context.Environment, module, "*");
        }
    }
}

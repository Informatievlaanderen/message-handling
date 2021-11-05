using System.Collections.Generic;
using System.Linq;

namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq.Definitions
{
    public sealed class QueueDefinition
    {
        public Environment Environment { get; }
        public MessageType MessageType { get; }
        public Module Module { get; }
        public RouteKey RouteKey { get; }
        public Exchange Exchange { get; }
        public string FullQueueName => RouteKey.Value;
        public QueueName QueueName { get; }
        public IList<string> WildcardBindings { get; }
        public IList<RouteKey> FullWildcardBindings { get; }
    
        public QueueDefinition(MessageType messageType, Environment environment,  Module module, QueueName name, IList<string> wildcardBindings = null)
        {
            Environment = environment;
            MessageType = messageType;
            Module = module;
            RouteKey = RouteKey.Create(messageType, environment, module, name.Value);
            Exchange = Exchange.Create(messageType, environment, module);
            QueueName = name;
            if (messageType == MessageType.Topic)
            {
                WildcardBindings = wildcardBindings;
                if (wildcardBindings != null)
                {
                    FullWildcardBindings = new List<RouteKey>();
                    wildcardBindings.ToList().ForEach(b => FullWildcardBindings.Add(RouteKey.Create(messageType, environment, module, b)));
                }
            }
        }
    }
}

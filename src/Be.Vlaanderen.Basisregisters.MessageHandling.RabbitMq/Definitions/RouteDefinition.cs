namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq.Definitions
{
    public sealed class RouteDefinition
    {
        public RouteDefinition(MessageType messageType, Environment environment,  Module module, string name)
        {
            Environment = environment;
            MessageType = messageType;
            Module = module;
            RouteKey = RouteKey.Create(messageType, environment, module, name);
            Exchange = Exchange.Create(messageType, environment, module);
        }
        public Environment Environment { get; }
        public MessageType MessageType { get; }
        public Module Module { get; }
        public RouteKey RouteKey { get; }
        public Exchange Exchange { get; }
    }
}

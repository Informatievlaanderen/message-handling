namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    using System.Collections.Generic;
    using RabbitMQ.Client;

    public class MessageHandlerContext
    {
        public IConnection Connection { get; }
        public Environment Environment { get; }
        public IList<Module> Modules { get; }
        public Module Module { get; }

        public MessageHandlerContext(IConnection connection,
            Environment environment,
            IList<Module> modules, Module module)
        {
            Connection = connection;
            Environment = environment;
            Modules = modules;
            Module = module;
        }
    }
}

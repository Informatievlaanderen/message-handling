namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    using System.Collections.Generic;
    using RabbitMQ.Client;

    public class MessageHandlerContext
    {
        public IConnection Connection { get; }
        public Environment Environment { get; }
        public IList<Module> OtherModules { get; }
        public Module ThisModule { get; }

        public MessageHandlerContext(
            IConnection connection,
            Environment environment,
            IList<Module> otherModules,
            Module thisModule)
        {
            Connection = connection;
            Environment = environment;
            OtherModules = otherModules;
            ThisModule = thisModule;
        }
    }
}

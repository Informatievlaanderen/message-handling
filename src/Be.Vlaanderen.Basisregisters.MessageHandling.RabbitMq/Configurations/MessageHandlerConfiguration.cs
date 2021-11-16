namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq.Configurations
{
    using System.Collections.Generic;

    public class MessageHandlerConfiguration
    {
        /// <summary>
        /// Config for RabbitMq
        /// </summary>
        public RabbitMqConfig RabbitMqConfig { get; set; }

        /// <summary>
        /// The environment (example: dev, test, ...), will be used in the queue name
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// List of all modules
        /// </summary>
        public IList<string> OtherModules { get; set; }

        /// <summary>
        /// The module name of the current project
        /// </summary>
        public string ThisModule { get; set; }
    }
}

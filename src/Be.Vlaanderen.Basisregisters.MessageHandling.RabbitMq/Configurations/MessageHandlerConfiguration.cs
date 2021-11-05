namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq.Configurations
{
    using System.Collections.Generic;

    public class MessageHandlerConfiguration
    {
        public RabbitMqConfig RabbitMq { get; set; }
        public string Environment { get; set; }

        /// <summary>
        /// List of all modules
        /// </summary>
        public IList<string> Modules { get; set; }

        /// <summary>
        /// The module name of the current project
        /// </summary>
        public string Module { get; set; }
    }
}

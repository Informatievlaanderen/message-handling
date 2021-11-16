namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq.Configurations
{
    using System;

    public class RabbitMqConfig
    {
        /// <summary>
        /// The URI for the RabbitMq
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// The port for the RabbitMq
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The username to log on to RabbitMq
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password to log on to RabbitMq
        /// </summary>
        public string Password { get; set; }
    }
}

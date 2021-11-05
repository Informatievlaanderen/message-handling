namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq.Configurations
{
    using System;

    public class RabbitMqConfig
    {
        public Uri Uri { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
}

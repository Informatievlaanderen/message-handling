namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Producer.Extensions
{
    using Confluent.Kafka;

    internal static class ProducerConfigExtensions
    {
        public static ProducerConfig WithAuthentication(this ProducerConfig config, ProducerOptions options)
        {
            if (options.SaslAuthentication.HasValue)
            {
                config.SecurityProtocol = SecurityProtocol.SaslSsl;
                config.SaslMechanism = SaslMechanism.Plain;
                config.SaslUsername = options.SaslAuthentication.Value.Username;
                config.SaslPassword = options.SaslAuthentication.Value.Password;
            }

            return config;
        }
    }
}

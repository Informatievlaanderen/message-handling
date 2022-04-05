using Confluent.Kafka;

namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple.Extensions
{
    public static class KafkaConfigExtensions
    {
        public static ConsumerConfig WithAuthentication(this ConsumerConfig config, KafkaOptions options)
        {
            if (!string.IsNullOrEmpty(options.SaslUserName))
            {
                config.SecurityProtocol = SecurityProtocol.SaslSsl;
                config.SaslMechanism = SaslMechanism.Plain;
                config.SaslUsername = options.SaslUserName;
                config.SaslPassword = options.SaslPassword;
            }

            return config;
        }

        public static ProducerConfig WithAuthentication(this ProducerConfig config, KafkaOptions options)
        {
            if (!string.IsNullOrEmpty(options.SaslUserName))
            {
                config.SecurityProtocol = SecurityProtocol.SaslSsl;
                config.SaslMechanism = SaslMechanism.Plain;
                config.SaslUsername = options.SaslUserName;
                config.SaslPassword = options.SaslPassword;
            }

            return config;
        }

        public static AdminClientConfig WithAuthentication(this AdminClientConfig config, KafkaOptions options)
        {
            if (!string.IsNullOrEmpty(options.SaslUserName))
            {
                config.SecurityProtocol = SecurityProtocol.SaslSsl;
                config.SaslMechanism = SaslMechanism.Plain;
                config.SaslUsername = options.SaslUserName;
                config.SaslPassword = options.SaslPassword;
            }

            return config;
        }
    }
}

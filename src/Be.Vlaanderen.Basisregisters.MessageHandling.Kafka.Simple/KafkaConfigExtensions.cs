namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    using Confluent.Kafka;

    public static class KafkaConfigExtensions
    {
        public static ConsumerConfig WithAuthentication(this ConsumerConfig consumerConfig, KafkaOptions options)
        {
            if (options.Authentication == KafkaAuthentication.SaslPlainText)
            {
                consumerConfig.SaslMechanism = SaslMechanism.Plain;
                consumerConfig.SaslUsername = options.SaslUserName;
                consumerConfig.SaslPassword = options.SaslPassword;
            }

            return consumerConfig;
        }

        public static ProducerConfig WithAuthentication(this ProducerConfig producerConfig, KafkaOptions options)
        {
            if (options.Authentication == KafkaAuthentication.SaslPlainText)
            {
                producerConfig.SaslMechanism = SaslMechanism.Plain;
                producerConfig.SaslUsername = options.SaslUserName;
                producerConfig.SaslPassword = options.SaslPassword;
            }

            return producerConfig;
        }
    }
}

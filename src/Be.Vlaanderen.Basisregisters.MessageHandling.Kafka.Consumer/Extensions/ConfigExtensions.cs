namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer.Extensions
{
    using System.Linq;
    using Confluent.Kafka;

    internal static class ConsumerConfigExtensions
    {
        public static ConsumerConfig WithAuthentication(this ConsumerConfig config, ConsumerOptions options)
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

        public static IConsumer<string, string> BuildConsumer(this ConsumerConfig config, ConsumerOptions options)
        {
            var consumerBuilder = new ConsumerBuilder<string, string>(config)
                .SetValueDeserializer(Deserializers.Utf8);
            if (options.Offset.HasValue)
            {
                consumerBuilder.SetPartitionsAssignedHandler((_, topicPartitions) =>
                {
                    var partitionOffset = topicPartitions.Select(x => new TopicPartitionOffset(x.Topic, x.Partition, new Offset(options.Offset.Value)));
                    return partitionOffset;
                });
            }

            return consumerBuilder.Build();
        }
    }
}

namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer
{
    using System;
    using Confluent.Kafka;
    using Extensions;
    using Newtonsoft.Json;
    using Offset = Offset;

    public class ConsumerOptions
    {
        public Topic Topic { get; }
        public BootstrapServers BootstrapServers { get; }
        public SaslAuthentication? SaslAuthentication { get; private set; }
        public JsonSerializerSettings JsonSerializerSettings { get; }
        public IMessageSerializer<string, string> MessageSerializer { get; }
        public ConsumerGroupId ConsumerGroupId { get; }

        /// <summary>
        /// Delay in milliseconds.
        /// When no new message was found before retrying to lookup next message.
        /// </summary>
        public int NoMessageFoundDelay { get; private set; } = 300;
        public Offset? Offset { get; private set; }

        public ConsumerOptions(
            BootstrapServers bootstrapServers,
            Topic topic,
            ConsumerGroupId consumerGroupId,
            JsonSerializerSettings? jsonSerializerSettings = null,
            IMessageSerializer<string, string>? messageSerializer = null)
        {
            BootstrapServers = bootstrapServers;
            Topic = topic;
            ConsumerGroupId = consumerGroupId;
            JsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
            MessageSerializer = messageSerializer ?? new JsonMessageSerializer(JsonSerializerSettings);
        }

        public ConsumerOptions ConfigureOffset(Offset offset)
        {
            Offset = offset;
            return this;
        }

        public ConsumerOptions ConfigureSaslAuthentication(SaslAuthentication saslAuthentication)
        {
            SaslAuthentication = saslAuthentication;
            return this;
        }

        public ConsumerOptions ConfigureNoMessageFoundDelay(int delayInMilliseconds)
        {
            if (delayInMilliseconds < 1)
            {
                throw new ArgumentException("Delay cannot be smaller than 1 millisecond.", nameof(delayInMilliseconds));
            }

            NoMessageFoundDelay = delayInMilliseconds;
            return this;
        }

        internal ConsumerConfig CreateConsumerConfig()
        {
            return new ConsumerConfig
            {
                BootstrapServers = BootstrapServers,
                GroupId = ConsumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            }.WithAuthentication(this);
        }
    }
}

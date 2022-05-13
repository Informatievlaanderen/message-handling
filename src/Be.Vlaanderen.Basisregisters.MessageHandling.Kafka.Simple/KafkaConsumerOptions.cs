namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    using System;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Newtonsoft.Json;

    public class KafkaConsumerOptions : KafkaOptions
    {
        public string ConsumerGroupId { get; }

        public string Topic { get; }

        /// <summary>
        /// Delay in milliseconds.
        /// When no new message was found before retrying to lookup next message.
        /// </summary>
        public int NoMessageFoundDelay { get; }

        public Func<object, Task> MessageHandler { get; }

        public Offset? Offset { get; }

        public KafkaConsumerOptions(
            string bootstrapServers,
            string consumerGroupId,
            string topic,
            Func<object, Task> messageHandler,
            int noMessageFoundDelay = 300,
            Offset? offset = null,
            JsonSerializerSettings? jsonSerializerSettings = null)
            : base(bootstrapServers, jsonSerializerSettings)
        {
            if (noMessageFoundDelay < 1)
            {
                throw new ArgumentException("Delay cannot be smaller than 1 millisecond.", nameof(noMessageFoundDelay));
            }

            ConsumerGroupId = consumerGroupId;
            Topic = topic;
            MessageHandler = messageHandler;
            NoMessageFoundDelay = noMessageFoundDelay;
            Offset = offset;
        }

        public KafkaConsumerOptions(
            string bootstrapServers,
            string userName,
            string password,
            string consumerGroupId,
            string topic,
            Func<object, Task> messageHandler,
            int noMessageFoundDelay = 300,
            Offset? offset = null,
            JsonSerializerSettings? jsonSerializerSettings = null)
            : base(bootstrapServers, userName, password, jsonSerializerSettings)
        {
            if (noMessageFoundDelay < 1)
            {
                throw new ArgumentException("Delay cannot be smaller than 1 millisecond.", nameof(noMessageFoundDelay));
            }

            ConsumerGroupId = consumerGroupId;
            Topic = topic;
            MessageHandler = messageHandler;
            NoMessageFoundDelay = noMessageFoundDelay;
            Offset = offset;
        }
    }
}

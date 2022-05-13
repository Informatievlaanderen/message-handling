namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    using Newtonsoft.Json;
    public class KafkaProducerOptions : KafkaOptions
    {
        public string Topic { get; }

        public KafkaProducerOptions(
            string bootstrapServers,
            string topic,
            JsonSerializerSettings? jsonSerializerSettings = null)
            : base(bootstrapServers, jsonSerializerSettings)
        {
            Topic = topic;
        }

        public KafkaProducerOptions(
            string bootstrapServers,
            string userName,
            string password,
            string topic,
            JsonSerializerSettings? jsonSerializerSettings = null)
            : base(bootstrapServers, userName, password, jsonSerializerSettings)
        {
            Topic = topic;
        }
    }
}

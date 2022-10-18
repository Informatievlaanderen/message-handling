namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    using Newtonsoft.Json;
    public class KafkaProducerOptions : KafkaOptions
    {
        public string Topic { get; }
        public bool UseSinglePartition { get; }

        public KafkaProducerOptions(
            string bootstrapServers,
            string topic,
            bool useSinglePartition = true,
            JsonSerializerSettings? jsonSerializerSettings = null)
            : base(bootstrapServers, jsonSerializerSettings)
        {
            Topic = topic;
            UseSinglePartition = useSinglePartition;
        }

        public KafkaProducerOptions(
            string bootstrapServers,
            string userName,
            string password,
            string topic,
            bool useSinglePartition = true,
            JsonSerializerSettings? jsonSerializerSettings = null)
            : base(bootstrapServers, userName, password, jsonSerializerSettings)
        {
            Topic = topic;
            UseSinglePartition = useSinglePartition;
        }
    }
}

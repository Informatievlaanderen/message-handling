namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    using Newtonsoft.Json;

    public class KafkaOptions 
    {
        public string BootstrapServers { get; }
        public JsonSerializerSettings JsonSerializerSettings { get; }

        public KafkaOptions(string bootstrapServers, JsonSerializerSettings? jsonSerializerSettings = null)
        {
            BootstrapServers = bootstrapServers;
            JsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
        }
    }
}

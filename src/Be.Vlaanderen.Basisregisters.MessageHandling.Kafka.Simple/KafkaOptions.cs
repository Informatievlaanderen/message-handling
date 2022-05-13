namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    using Newtonsoft.Json;

    public class KafkaOptions
    {
        public string BootstrapServers { get; }
        public string SaslUserName { get; }
        public string SaslPassword { get; }
        public JsonSerializerSettings JsonSerializerSettings { get; }

        public KafkaOptions(string bootstrapServers, JsonSerializerSettings? jsonSerializerSettings = null)
        {
            BootstrapServers = bootstrapServers;
            SaslUserName = string.Empty;
            SaslPassword = string.Empty;
            JsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
        }

        public KafkaOptions(string bootstrapServers, string userName, string password, JsonSerializerSettings? jsonSerializerSettings = null)
        {
            BootstrapServers = bootstrapServers;
            SaslUserName = userName;
            SaslPassword = password;
            JsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
        }
    }
}

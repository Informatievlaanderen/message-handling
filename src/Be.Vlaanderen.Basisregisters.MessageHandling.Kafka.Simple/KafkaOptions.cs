namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    using Newtonsoft.Json;

    public class KafkaOptions 
    {
        public string BootstrapServers { get; }
        public KafkaAuthentication Authentication { get; }
        public string SaslUserName {get;}
        public string SaslPassword { get; }
        public JsonSerializerSettings JsonSerializerSettings { get; }

        public KafkaOptions(string bootstrapServers, JsonSerializerSettings? jsonSerializerSettings = null)
        {
            BootstrapServers = bootstrapServers;
            JsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
        }

        public KafkaOptions(string bootstrapServers, KafkaAuthentication authentication, string userName, string password, JsonSerializerSettings? jsonSerializerSettings = null)
        {
            BootstrapServers = bootstrapServers;
            Authentication = authentication;
            SaslUserName = userName;
            SaslPassword = password;
            JsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
        }
    }
}

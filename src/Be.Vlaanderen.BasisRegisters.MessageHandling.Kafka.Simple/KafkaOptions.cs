namespace Be.Vlaanderen.BasisRegisters.MessageHandling.Kafka.Simple
{
    using Confluent.SchemaRegistry.Serdes;

    public class KafkaOptions 
    {
        public string BootstrapServers { get; set; }
        public string SchemaRegistryUrl { get; set; }
        public JsonSerializerConfig JsonSerializerConfig { get; set; }
        public JsonDeserializerConfig JsonDeserializerConfig { get; set; }

        public KafkaOptions(string bootstrapServers, string schemaRegistryUrl, JsonSerializerConfig? jsonSerializerConfig = null, JsonDeserializerConfig? jsonDeserializerConfig = null)
        {
            BootstrapServers = bootstrapServers;
            SchemaRegistryUrl = schemaRegistryUrl;
            JsonSerializerConfig = jsonSerializerConfig ?? new JsonSerializerConfig();
            JsonDeserializerConfig = jsonDeserializerConfig ?? new JsonDeserializerConfig();
        }
    }
}

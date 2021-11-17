namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Configurations
{
    using System.Collections.Generic;
    using Confluent.Kafka.Admin;

    public class KafkaConfiguration
    {
        public string Environment { get; set; }
        public string? ProducerName {get;set;}
        public Dictionary<string, string> ClientConfig { get; set; }
        public List<TopicSpecification>? ProduceTopics { get; set; }
    }
}

namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configurations;
    using Confluent.Kafka;
    using Confluent.Kafka.Admin;

    public class KafkaContext
    {
        public ClientConfig ClientConfig { get; }
        public Dictionary<TopicName, TopicSpecification> ProduceTopics { get; }
        public Environment Environment { get; }
        public ProducerName? ProducerName { get; }

        public KafkaContext(
            ClientConfig clientConfig,
            IList<TopicSpecification>? topicSpecifications,
            Environment environment,
            ProducerName? producerName)
        {
            ClientConfig = clientConfig;
            ProduceTopics = ToDictionary(topicSpecifications, environment, producerName);
            Environment = environment;
            ProducerName = producerName;
        }

        private Dictionary<TopicName, TopicSpecification> ToDictionary(IList<TopicSpecification>? topicSpecifications, Environment environment, ProducerName? producerName)
        {
            var topics = new Dictionary<TopicName, TopicSpecification>();
            if (topicSpecifications == null || !topicSpecifications.Any())
                return topics;

            if (string.IsNullOrWhiteSpace(producerName))
                throw new Exception("ProducerName is missing");

            foreach (var topic in topicSpecifications)
            {
                var key = new TopicName(topic.Name);
                topic.Name = $"{environment}-{producerName}-{key}";
                topics.Add(key, topic);
            }

            return topics;
        }
    }
}

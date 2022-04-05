using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple.Extensions;
using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    public static class KafkaTopic
    {
        public static IEnumerable<string> ListTopicNames(KafkaOptions options)
        {
            using var client = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = options.BootstrapServers }.WithAuthentication(options)).Build();

            var metadata = client.GetMetadata(TimeSpan.FromSeconds(10));
            var topicNames = metadata.Topics.Select(x => x.Topic);

            return topicNames;
        }

        public static async Task CreateTopic(KafkaOptions options, string topic, int retentionMilliSeconds = -1, long retentionBytes = -1)
        {
            using var client = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = options.BootstrapServers }.WithAuthentication(options)).Build();

            await client.CreateTopicsAsync(new []{ new TopicSpecification
            {
                Name = topic,
                NumPartitions = 1,
                ReplicationFactor = 1,
                Configs = new Dictionary<string, string>
                {
                    ["retention.ms"] = retentionMilliSeconds.ToString(),
                    ["retention.bytes"] = retentionBytes.ToString()
                }
            }});
        }

        public static async Task DeleteTopic(KafkaOptions options, string topic)
        {
            using var client = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = options.BootstrapServers }.WithAuthentication(options)).Build();

            await client.DeleteTopicsAsync(new[] { topic });
        }

        public static async Task<string> GetTopicConfigurationItem(KafkaOptions options, string topic, string item)
        {
            using var client = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = options.BootstrapServers }.WithAuthentication(options)).Build();

            var results = await client.DescribeConfigsAsync(new []
            {
                new ConfigResource
                {
                    Name = topic,
                    Type = ResourceType.Topic
                }
            });

            return results.SelectMany(x => x.Entries)
                .FirstOrDefault(x => x.Key.Equals(item, StringComparison.InvariantCultureIgnoreCase))
                .Value
                .Value;
        }

        public static async Task<string> GetTopicRetentionMilliseconds(KafkaOptions options, string topic) => await GetTopicConfigurationItem(options, topic, "retention.ms");
    }
}

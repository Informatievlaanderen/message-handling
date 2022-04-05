using System.Linq;

namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
using System;
using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Extensions;
    using Newtonsoft.Json;

    public static class KafkaConsumer
    {
        public static async Task<Result<KafkaJsonMessage>> Consume(
            KafkaOptions options,
            string consumerGroupId,
            string topic,
            Func<object, Task> messageHandler,
            Offset? offset = null,
            CancellationToken cancellationToken = default)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = options.BootstrapServers,
                GroupId = consumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            }.WithAuthentication(options);

            var serializer = JsonSerializer.CreateDefault(options.JsonSerializerSettings);

            var consumerBuilder = new ConsumerBuilder<Ignore, string>(config)
                .SetValueDeserializer(Deserializers.Utf8);
            if (offset.HasValue)
            {
                consumerBuilder.SetPartitionsAssignedHandler((cons, topicPartitions) =>
                {
                    var partitionOffset = topicPartitions.Select(x => new TopicPartitionOffset(x.Topic, x.Partition, offset.Value));
                    return partitionOffset;
                });
            }

            using var consumer = consumerBuilder.Build();
            try
            {
                consumer.Subscribe(topic);

                var kafkaJsonMessage = new KafkaJsonMessage("", "");
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(TimeSpan.FromSeconds(3));
                    if (consumeResult == null) //if no message is found, returns null
                    {
                        break;
                    }

                    kafkaJsonMessage = serializer.Deserialize<KafkaJsonMessage>(consumeResult.Message.Value) ?? throw new ArgumentException("Kafka json message is null.");
                    var messageData = kafkaJsonMessage.Map() ?? throw new ArgumentException("Kafka message data is null.");

                    await messageHandler(messageData);
                    consumer.Commit(consumeResult);
                }

                return Result<KafkaJsonMessage>.Success(kafkaJsonMessage);
            }
            catch (ConsumeException ex)
            {
                return Result<KafkaJsonMessage>.Failure(ex.Error.Code.ToString(), ex.Error.Reason);
            }
            catch (OperationCanceledException)
            {
                return Result<KafkaJsonMessage>.Success(null);
            }
            finally
            {
                consumer.Unsubscribe();
            }
        }
    }
}

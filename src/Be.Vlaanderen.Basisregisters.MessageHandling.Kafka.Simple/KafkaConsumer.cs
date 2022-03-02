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
        public static async Task<Result> Consume(
            KafkaOptions options,
            string consumerGroupId,
            string topic,
            Func<object, Task> messageHandler,
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

            using var consumer = new ConsumerBuilder<Ignore, string>(config)
                .SetValueDeserializer(Deserializers.Utf8)
                .Build();
            try
            {
                consumer.Subscribe(topic);

                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(TimeSpan.FromSeconds(3));
                    if (consumeResult == null) //if no message is found, returns null
                    {
                        continue;
                    }

                    var kafkaJsonMessage = serializer.Deserialize<KafkaJsonMessage>(consumeResult.Message.Value) ?? throw new ArgumentException("Kafka json message is null.");
                    var messageData = kafkaJsonMessage.Map() ?? throw new ArgumentException("Kafka message data is null.");

                    await messageHandler(messageData);
                    consumer.Commit(consumeResult);
                }

                return Result.Success();
            }
            catch (ConsumeException ex)
            {
                return Result.Failure(ex.Error.Code.ToString(), ex.Error.Reason);
            }
            catch (OperationCanceledException)
            {
                return Result.Success();
            }
            finally
            {
                consumer.Unsubscribe();
            }
        }
    }
}

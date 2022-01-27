namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    using System;
    using System.Threading;
    using Confluent.Kafka;
    using Extensions;
    using Newtonsoft.Json;

    public static class KafkaConsumer
    {
        public static Result Consume<T>(
            KafkaOptions options,
            string consumerGroupId,
            string topic,
            Action<T> messageHandler,
            CancellationToken cancellationToken = default)
            where T: class
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = options.BootstrapServers,
                GroupId = consumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

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

                    var kafkaJsonMessage = serializer.Deserialize<KafkaJsonMessage>(consumeResult.Message.Value) ?? throw new ArgumentException("Kafka json message is null.");
                    var messageData = kafkaJsonMessage.Map<T>() ?? throw new ArgumentException("Kafka message data is null.");

                    messageHandler?.Invoke(messageData);
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

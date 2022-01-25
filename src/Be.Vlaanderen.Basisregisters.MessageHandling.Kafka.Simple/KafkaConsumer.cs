namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    using System;
    using System.Threading;
    using Confluent.Kafka;
    using Confluent.Kafka.SyncOverAsync;
    using Confluent.SchemaRegistry.Serdes;

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

            using var consumer = new ConsumerBuilder<Ignore, T>(config)
                .SetValueDeserializer(new JsonDeserializer<T>(options.JsonDeserializerConfig).AsSyncOverAsync())
                .Build();
            try
            {
                consumer.Subscribe(topic);

                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    messageHandler?.Invoke(consumeResult.Message.Value);
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

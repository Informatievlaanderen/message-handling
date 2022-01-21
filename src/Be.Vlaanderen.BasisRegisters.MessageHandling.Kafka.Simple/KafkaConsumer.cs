using System;
using System.Threading;
using Confluent.Kafka;

namespace Be.Vlaanderen.BasisRegisters.MessageHandling.Kafka.Simple
{
    public static class KafkaConsumer
    {
        public static Result ConsumeAll<T>(string bootstrapServers, string consumerGroupId, string topic, Action<T> messageHandler, CancellationToken cancellationToken = default)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = consumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, T>(config)
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

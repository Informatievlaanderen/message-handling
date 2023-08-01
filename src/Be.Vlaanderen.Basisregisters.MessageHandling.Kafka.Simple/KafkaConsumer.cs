namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Extensions;
    using Newtonsoft.Json;

    public static class KafkaConsumer
    {
        public static Task<Result<KafkaJsonMessage>> Consume(
            KafkaConsumerOptions options,
            CancellationToken cancellationToken = default)
        {
            return new KafkaJsonMessageConsumer(options)
                .Consume(cancellationToken);
        }

        public static Offset Position(KafkaConsumerOptions options)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = options.BootstrapServers,
                GroupId = options.ConsumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            }.WithAuthentication(options);

            using var consumer = new ConsumerBuilder<Ignore, string>(config)
                .SetValueDeserializer(Deserializers.Utf8)
                .Build();
            return consumer.Position(new TopicPartition(options.Topic, 0));
        }
    }

    public class KafkaConsumer<TMessage, TKey>
        where TMessage : new()
    {
        public KafkaConsumer(KafkaConsumerOptions options)
        {
            Options = options;
            Serializer = JsonSerializer.CreateDefault(options.JsonSerializerSettings);
        }

        protected KafkaConsumerOptions Options { get; }
        protected JsonSerializer Serializer { get; }

        public async Task<Result<TMessage>> Consume(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = Options.BootstrapServers,
                GroupId = Options.ConsumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            }.WithAuthentication(Options);

            var consumerBuilder = new ConsumerBuilder<TKey, string>(config)
                .SetValueDeserializer(Deserializers.Utf8);
            if (Options.Offset.HasValue)
            {
                consumerBuilder.SetPartitionsAssignedHandler((cons, topicPartitions) =>
                {
                    var partitionOffset = topicPartitions.Select(x => new TopicPartitionOffset(x.Topic, x.Partition, Options.Offset.Value));
                    return partitionOffset;
                });
            }

            var kafkaMessage = new TMessage();
            using var consumer = consumerBuilder.Build();
            try
            {
                consumer.Subscribe(Options.Topic);

                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(TimeSpan.FromSeconds(3));
                    if (consumeResult == null) //if no message is found, returns null
                    {
                        await Task.Delay(Options.NoMessageFoundDelay, cancellationToken);
                        continue;
                    }

                    kafkaMessage = ConvertConsumeResultToMessage(consumeResult) ?? throw new ArgumentException("Kafka message is null.");
                    await HandleMessageAsync(kafkaMessage);

                    consumer.Commit(consumeResult);
                }

                return Result<TMessage>.Success(kafkaMessage);
            }
            catch (ConsumeException ex)
            {
                return Result<TMessage>.Failure(ex.Error.Code.ToString(), ex.Error.Reason);
            }
            catch (OperationCanceledException)
            {
                return Result<TMessage>.Success(kafkaMessage);
            }
            finally
            {
                consumer.Unsubscribe();
            }
        }

        protected virtual TMessage? ConvertConsumeResultToMessage(ConsumeResult<TKey, string> consumeResult)
        {
            return Serializer.Deserialize<TMessage>(consumeResult.Message.Value);
        }

        protected virtual Task HandleMessageAsync([DisallowNull] TMessage message)
        {
            return Options.MessageHandler(message);
        }
    }
}

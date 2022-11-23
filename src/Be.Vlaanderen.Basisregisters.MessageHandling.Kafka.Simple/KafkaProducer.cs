namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Extensions;
    using Newtonsoft.Json;

    public static class KafkaProducer
    {
        public static async Task<Result> Produce(
            KafkaProducerOptions options,
            string key,
            string message,
            CancellationToken cancellationToken = default)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = options.BootstrapServers,
                ClientId = Dns.GetHostName()
            }.WithAuthentication(options);

            try
            {
                await ProduceMessage(options, config, key, message, cancellationToken);

                return Result.Success();
            }
            catch (ProduceException<Null, string> ex)
            {
                return Result.Failure(ex.Error.Code.ToString(), ex.Error.Reason);
            }
            catch (OperationCanceledException ex)
            {
                return Result.Failure(ex.Message, ex.Message);
            }
        }

        public static async Task<Result<T>> Produce<T>(
            KafkaProducerOptions options,
            string key,
            T message,
            CancellationToken cancellationToken = default)
            where T : class
        {
            var config = new ProducerConfig
            {
                BootstrapServers = options.BootstrapServers,
                ClientId = Dns.GetHostName()
            }.WithAuthentication(options);

            try
            {
                var serializer = JsonSerializer.CreateDefault(options.JsonSerializerSettings);
                var kafkaJsonMessage = KafkaJsonMessage.Create(message, serializer);

                await ProduceMessage(options, config, key, serializer.Serialize(kafkaJsonMessage), cancellationToken);
                return Result<T>.Success(message);
            }
            catch (ProduceException<Null, T> ex)
            {
                return Result<T>.Failure(ex.Error.Code.ToString(), ex.Error.Reason);
            }
            catch (OperationCanceledException)
            {
                return Result<T>.Success(default);
            }
        }

        private static async Task ProduceMessage(
            KafkaProducerOptions options,
            ProducerConfig config,
            string key,
            string message,
            CancellationToken cancellationToken)
        {
            using var producer = new ProducerBuilder<string, string>(config)
                .SetKeySerializer(Serializers.Utf8)
                .SetValueSerializer(Serializers.Utf8)
                .Build();

            if (options.UseSinglePartition)
            {
                _ = await producer.ProduceAsync(new TopicPartition(options.Topic, new Partition(0)),
                    new Message<string, string> { Key = key, Value = message }, cancellationToken);
            }
            else
            {
                _ = await producer.ProduceAsync(options.Topic, new Message<string, string> { Key = key, Value = message }, cancellationToken);
            }
        }
    }
}

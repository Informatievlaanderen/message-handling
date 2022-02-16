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
        public static async Task<Result<T>> Produce<T>(
            KafkaOptions options,
            string topic,
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

                using var producer = new ProducerBuilder<string, string>(config)
                    .SetKeySerializer(Serializers.Utf8)
                    .SetValueSerializer(Serializers.Utf8)
                    .Build();

                _ = await producer.ProduceAsync(new TopicPartition(topic, new Partition(0)), new Message<string, string> { Key = key, Value = serializer.Serialize(kafkaJsonMessage) }, cancellationToken);
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
    }
}

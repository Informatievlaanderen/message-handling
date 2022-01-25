namespace Be.Vlaanderen.BasisRegisters.MessageHandling.Kafka.Simple
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Confluent.SchemaRegistry;
    using Confluent.SchemaRegistry.Serdes;

    public static class KafkaProducer
    {
        public static async Task<Result<T>> Produce<T>(
            KafkaOptions options,
            string topic,
            T message,
            CancellationToken cancellationToken = default)
            where T : class
        {
            var config = new ProducerConfig
            {
                BootstrapServers = options.BootstrapServers,
                ClientId = Dns.GetHostName()
            };

            var schemaConfig = new SchemaRegistryConfig
            {
                Url = options.SchemaRegistryUrl
            };

            try
            {
                using var schemaRegistryClient = new CachedSchemaRegistryClient(schemaConfig);
                using var producer = new ProducerBuilder<Null, T>(config)
                    .SetValueSerializer(new JsonSerializer<T>(schemaRegistryClient, options.JsonSerializerConfig))
                    .Build();

                _ = await producer.ProduceAsync(topic, new Message<Null, T> { Value = message }, cancellationToken);
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

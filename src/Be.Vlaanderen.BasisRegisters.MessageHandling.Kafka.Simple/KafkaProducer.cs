using System.Net;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Be.Vlaanderen.BasisRegisters.MessageHandling.Kafka.Simple
{
    public static class KafkaProducer
    {
        public static async Task<Result> Produce<T>(string bootstrapServers, string topic, T message)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = Dns.GetHostName()
            };

            try
            {
                using var producer = new ProducerBuilder<Null, T>(config)
                    .Build();

                _ = await producer.ProduceAsync(topic, new Message<Null, T> { Value = message });
                return Result<T>.Success(message);
            }
            catch (ProduceException<Null, T> ex)
            {
                return Result.Failure(ex.Error.Code.ToString(), ex.Error.Reason);
            }
        }
    }
}

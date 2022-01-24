using System;
using System.Threading.Tasks;
using Be.Vlaanderen.BasisRegisters.MessageHandling.Kafka.Simple;
using Microsoft.Extensions.Configuration;

namespace SampleSimpleKafkaPublisher
{
    public static class Program
    {
        public static Task Main()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var bootstrapServers = config["Kafka:BootstrapServers"];
            return KafkaProducer.Produce(bootstrapServers, "topic-1", $"Hello at {DateTimeOffset.Now}");
        }
    }
}

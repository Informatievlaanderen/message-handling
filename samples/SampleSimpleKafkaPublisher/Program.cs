namespace SampleSimpleKafkaPublisher
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.BasisRegisters.MessageHandling.Kafka.Simple;
    using Microsoft.Extensions.Configuration;
    using SampleSimpleKafkaMessages;

    public static class Program
    {
        public static async Task Main()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
                Console.WriteLine("Stopping");
            };

            while (!cancellationToken.IsCancellationRequested)
            {
                const string topic = "topic-1";
                var result = await KafkaProducer.Produce(config["Kafka:BootstrapServers"], config["Kafka:SchemaRegistryUrl"], topic, new SomeSimpleMessage($"Hello at {DateTimeOffset.Now}"), cancellationToken);

                Console.WriteLine($"Put this on topic {topic}: {result.Message}");
                Console.WriteLine("Press Enter to produce a message");
                Console.ReadLine();
            }
        }
    }
}

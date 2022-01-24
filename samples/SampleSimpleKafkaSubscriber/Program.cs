using System;
using System.Threading;
using Be.Vlaanderen.BasisRegisters.MessageHandling.Kafka.Simple;
using Microsoft.Extensions.Configuration;

namespace SampleSimpleKafkaSubscriber
{
    public static class Program
    {
        public static void Main()
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

            _ = KafkaConsumer.ConsumeAll<string>(config["Kafka:BootstrapServers"], "consumer-group-1", "topic-1", message =>
            {
                Console.WriteLine($"Received: {message}");
            }, cancellationToken);
        }
    }
}

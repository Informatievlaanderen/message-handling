using System;
using System.Threading;
using Be.Vlaanderen.BasisRegisters.MessageHandling.Kafka.Simple;
using Microsoft.Extensions.Configuration;
using SampleSimpleKafkaMessages;

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

            const string consumerGroup = "consumer-group-1";
            _ = KafkaConsumer.Consume<SomeSimpleMessage>(config["Kafka:BootstrapServers"], consumerGroup, "topic-1", message =>
            {
                Console.WriteLine($"Consumer group {consumerGroup} received: {message}");
            }, cancellationToken);
        }
    }
}

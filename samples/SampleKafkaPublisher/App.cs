namespace SampleKafkaPublisher
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Microsoft.Extensions.Logging;

    public class App
    {
        private readonly MunicipalityEventProducer _municipalityEventProducer;
        private readonly ILogger<App> _logger;

        public App(MunicipalityEventProducer municipalityEventProducer,
            ILoggerFactory loggerFactory)
        {
            _municipalityEventProducer = municipalityEventProducer;
            _logger = loggerFactory.CreateLogger<App>();
        }

        public async Task Run()
        {
            Console.Write("Kafka Publisher");
            var message = new Message<string, string>()
            {
                Timestamp = Timestamp.Default,
                Key = Guid.NewGuid().ToString("D"),
                Value = "Hello-world !!!"
            };
            var watch = new System.Diagnostics.Stopwatch();
            for (int i = 0; i < 100; i++)
            {
                watch.Reset();
                watch.Start();
                await _municipalityEventProducer.PublishAsync(message);
                watch.Stop();
                Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
            }

            Console.ReadKey();
        }
    }
}

namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Extensions;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public sealed class Consumer : IConsumer, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IConsumer<string, string> _consumer;
        private readonly JsonSerializer _serializer;

        public ConsumerOptions ConsumerOptions { get; }

        internal Consumer(
            ConsumerOptions consumerOptions,
            IConsumer<string, string> consumer,
            ILoggerFactory loggerFactory)
        {
            ConsumerOptions = consumerOptions;
            _consumer = consumer;
            _serializer = JsonSerializer.CreateDefault(ConsumerOptions.JsonSerializerSettings);
            _logger = loggerFactory.CreateLogger<Consumer>();
        }

        public Consumer(
            ConsumerOptions consumerOptions,
            ILoggerFactory loggerFactory)
        : this(
            consumerOptions,
            consumerOptions.CreateConsumerConfig().BuildConsumer(consumerOptions),
            loggerFactory)
        { }

        public async Task ConsumeContinuously(Func<object, Task> messageHandler, CancellationToken cancellationToken = default)
        {
            try
            {
                _consumer.Subscribe(ConsumerOptions.Topic);
                _logger.LogInformation($"Subscribed to {ConsumerOptions.Topic}");

                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(3));
                    if (consumeResult == null) //if no message is found, returns null
                    {
                        await Task.Delay(ConsumerOptions.NoMessageFoundDelay, cancellationToken);
                        continue;
                    }

                    var kafkaJsonMessage = _serializer.Deserialize<JsonMessage>(consumeResult.Message.Value)
                                           ?? throw new ArgumentException("Kafka json message is null.");
                    var messageData = kafkaJsonMessage.Map()
                                      ?? throw new ArgumentException("Kafka message data is null.");

                    await messageHandler(messageData);

                    _consumer.Commit(consumeResult);
                }
            }
            finally
            {
                _logger.LogInformation("Unsubscribing...");
                _consumer.Unsubscribe();
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _consumer.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Consumer()
        {
            Dispose(false);
        }
    }
}

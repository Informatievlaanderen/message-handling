namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Offset = Kafka.Offset;

    public sealed class IdempotentConsumer<TConsumerContext> : IIdempotentConsumer<TConsumerContext>, IDisposable
        where TConsumerContext : ConsumerDbContext<TConsumerContext>
    {
        private readonly IDbContextFactory<TConsumerContext> _dbContextFactory;
        private readonly ILogger _logger;
        private readonly JsonSerializer _serializer;
        private readonly IConsumer<string, string> _consumer;

        public ConsumerName ConsumerName { get; }
        public ConsumerOptions ConsumerOptions { get; }

        internal IdempotentConsumer(
            ConsumerName consumerName,
            ConsumerOptions consumerOptions,
            IConsumer<string, string> consumer,
            IDbContextFactory<TConsumerContext> dbContextFactory,
            ILoggerFactory loggerFactory)
        {
            ConsumerName = consumerName;
            ConsumerOptions = consumerOptions;
            _consumer = consumer;
            _dbContextFactory = dbContextFactory;
            _serializer = JsonSerializer.CreateDefault(ConsumerOptions.JsonSerializerSettings);
            _logger = loggerFactory.CreateLogger<Consumer>();
        }

        public IdempotentConsumer(
            ConsumerName consumerName,
            ConsumerOptions consumerOptions,
            IDbContextFactory<TConsumerContext> dbContextFactory,
            ILoggerFactory loggerFactory)
            : this(
                consumerName,
                consumerOptions,
                consumerOptions.CreateConsumerConfig().BuildConsumer(consumerOptions),
                dbContextFactory,
                loggerFactory)
        { }

        public async Task ConsumeContinuously(Func<object, TConsumerContext, Task> messageHandler, CancellationToken cancellationToken = default)
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

                    var idempotenceKey = consumeResult.Message.Headers.TryGetLastBytes(MessageHeader.IdempotenceKey,
                        out var idempotenceHeaderAsBytes)
                        ? Encoding.UTF8.GetString(idempotenceHeaderAsBytes)
                        : consumeResult.Message.Value.ToSha512();

                    await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                    var messageAlreadyProcessed = await dbContext.ProcessedMessages
                        .AsNoTracking()
                        .AnyAsync(x => x.IdempotenceKey == idempotenceKey, cancellationToken)
                        .ConfigureAwait(false);

                    if (messageAlreadyProcessed)
                    {
                        _logger.LogWarning(
                            $"Skipping already processed message at offset '{consumeResult.Offset.Value}' with idempotenceKey '{idempotenceKey}'.");
                        _consumer.Commit(consumeResult);
                        continue;
                    }

                    var processedMessage = new ProcessedMessage(idempotenceKey, DateTimeOffset.Now);

                    try
                    {
                        await dbContext.ProcessedMessages
                            .AddAsync(processedMessage, cancellationToken)
                            .ConfigureAwait(false);

                        await dbContext.UpdateConsumerState(
                                ConsumerName,
                                new Offset(consumeResult.Offset),
                                cancellationToken)
                            .ConfigureAwait(false);

                        await dbContext.SaveChangesAsync(cancellationToken)
                            .ConfigureAwait(false);

                        await messageHandler(messageData, dbContext);

                        _consumer.Commit(consumeResult);
                    }
                    catch (Exception ex)
                    {
                        dbContext.ProcessedMessages.Remove(processedMessage);
                        await dbContext.Error(ConsumerName, ex.Message, CancellationToken.None);
                        await dbContext.SaveChangesAsync(CancellationToken.None);
                        throw;
                    }
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

        ~IdempotentConsumer()
        {
            Dispose(false);
        }
    }
}

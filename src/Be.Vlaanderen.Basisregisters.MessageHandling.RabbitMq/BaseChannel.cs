namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    using System;
    using System.Text.Json;
    using System.Collections.Generic;
    using System.Linq;
    using Definitions;
    using RabbitMQ.Client;
    using Polly;

    /// <summary>
    /// Make sure you create a new instance each thread.
    /// </summary>
    public abstract class BaseChannel : IDisposable
    {
        private bool disposed = false;
        private readonly int _maxRetry;
        protected readonly MessageHandlerContext _context;

        protected IModel? Channel { get; private set; }
        protected IBasicProperties BasicProperties { get; private set; }

        protected BaseChannel(MessageHandlerContext context, int maxRetry = 5)
        {
            _context = context;
            _maxRetry = Math.Max(0, maxRetry);
            EnsureOpenChannel();
        }

        protected ReadOnlyMemory<byte> SerializeToUTF8Bytes<T>(T message)
            => new ReadOnlyMemory<byte>(JsonSerializer.SerializeToUtf8Bytes(message));

        protected void EnsureOpenChannel()
        {
            if (Channel == null)
            {
                Channel = _context.Connection.CreateModel();
                //Make queues durable and store to disk
                BasicProperties = Channel.CreateBasicProperties();
                BasicProperties.Persistent = true;
                BasicProperties.DeliveryMode = 2;
            }
            else if (Channel.IsClosed)
            {
                Channel.Dispose();
                Channel = null;
                EnsureOpenChannel();
            }
        }

        protected void EnsureExchangeExists(Exchange exchange, MessageType type)
        {
            Policy.Handle<Exception>()
                .Retry(_maxRetry, (ex, count) =>
                {
                    if (!ex.Message.Contains("no exchange"))
                        throw ex;
                    EnsureOpenChannel();
                    Channel!.ExchangeDeclare(exchange, type, true, false, null);
                })
                .Execute(() =>
                {
                    EnsureOpenChannel();
                    Channel!.ExchangeDeclarePassive(exchange);
                });
        }

        private void EnsureDeadLetterQueueExists(QueueDefinition definition)
        {
            Policy.Handle<Exception>()
                .Retry(_maxRetry, (ex, count) =>
                {
                    if (!ex.Message.Contains("no queue"))
                        throw ex;
                    EnsureOpenChannel();
                    Channel!.QueueDeclare(definition.DlxName, true, false, false, new Dictionary<string, object>
                    {
                        {"x-dead-letter-exchange", definition.Exchange.Value},
                        {"x-dead-letter-routing-key", definition.FullQueueName},
                        {"x-message-ttl", 30000}
                    });
                    ApplyDeadLetterQueueBindings(definition);
                })
                .Execute(() =>
                {
                    EnsureOpenChannel();
                    Channel!.QueueDeclarePassive(definition.DlxName);
                });
        }

        protected void EnsureQueueExists(QueueDefinition definition)
        {
            EnsureExchangeExists(definition.Exchange, definition.MessageType);
            Policy.Handle<Exception>()
                .Retry(_maxRetry, (ex, count) =>
                {
                    if (!ex.Message.Contains("no queue"))
                        throw ex;
                    EnsureOpenChannel();
                    Channel!.QueueDeclare(definition.FullQueueName, true, false, false, new Dictionary<string, object>
                    {
                        {"x-dead-letter-exchange", definition.Exchange.Value},
                        {"x-dead-letter-routing-key", definition.DlxName}
                    });
                    ApplyBindings(definition);
                })
                .Execute(() =>
                {
                    EnsureOpenChannel();
                    Channel!.QueueDeclarePassive(definition.FullQueueName);
                });
            EnsureDeadLetterQueueExists(definition);
        }

        private void ApplyBindings(QueueDefinition definition)
        {
            Channel!.QueueBind(definition.FullQueueName, definition.Exchange, definition.BindingKey);
        }

        private void ApplyDeadLetterQueueBindings(QueueDefinition definition)
        {
            Channel!.QueueBind(definition.DlxName, definition.Exchange, definition.DlxName);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposeManaged)
        {
            if (disposed) return;

            if (disposeManaged)
            {
                Channel.Dispose();
            }

            disposed = true;
        }
    }
}

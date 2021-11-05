using System;
using System.Text.Json;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Linq;
using Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq.Definitions;

namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    /// <summary>
    /// Make sure you create a new instance each thread.
    /// </summary>
    public abstract class BaseChannel : IDisposable
    {
        private bool disposed = false;
        protected IConnection _connection;
        protected IModel? Channel { get; private set; }
        protected IBasicProperties BasicProperties { get; private set; }

        protected BaseChannel(IConnection connection)
        {
            _connection = connection;
            EnsureOpenChannel();
        }

        protected ReadOnlyMemory<byte> SerializeToUTF8Bytes<T>(T message)
            => new ReadOnlyMemory<byte>(JsonSerializer.SerializeToUtf8Bytes(message));

        protected void EnsureOpenChannel()
        {
            if (Channel == null)
            {
                Channel = _connection.CreateModel();
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
            ProcessHandler.Retry(() =>
            {
                try
                {
                    EnsureOpenChannel();
                    Channel!.ExchangeDeclarePassive(exchange);
                }
                catch (Exception e)
                {
                    if (!e.Message.Contains("no exchange"))
                        throw;
                    EnsureOpenChannel();
                    Channel!.ExchangeDeclare(exchange, type, true, false, null);
                }
            }, 5);

        }

        private void EnsureDeadLetterQueueExists(QueueDefinition definition)
        {
            ProcessHandler.Retry(() =>
            {
                try
                {
                    EnsureOpenChannel();
                    Channel!.QueueDeclarePassive($"dlx.{definition.FullQueueName}");
                }
                catch (Exception e)
                {
                    if (!e.Message.Contains("no queue"))
                        throw;
                    EnsureOpenChannel();
                    Channel!.QueueDeclare($"dlx.{definition.FullQueueName}", true, false, false, new Dictionary<string, object>
                    {
                        {"x-dead-letter-exchange", definition.Exchange.Value},
                        {"x-dead-letter-routing-key", definition.FullQueueName},
                        {"x-message-ttl", 30000}
                    });
                    ApplyDeadLetterQueueBindings(definition);
                }
            }, 5);
        }

        protected void EnsureQueueExists(QueueDefinition definition)
        {
            EnsureExchangeExists(definition.Exchange, definition.MessageType);
            ProcessHandler.Retry(() =>
            {
                try
                {
                    EnsureOpenChannel();
                    Channel!.QueueDeclarePassive(definition.FullQueueName);
                }
                catch (Exception e)
                {
                    if (!e.Message.Contains("no queue"))
                        throw;
                    EnsureOpenChannel();
                    Channel!.QueueDeclare(definition.FullQueueName, true, false, false, new Dictionary<string, object>
                    {
                        {"x-dead-letter-exchange", definition.Exchange.Value},
                        {"x-dead-letter-routing-key", $"dlx.{definition.FullQueueName}"}
                    });
                    ApplyBindings(definition);
                }
            }, 5);
            EnsureDeadLetterQueueExists(definition);
        }

        private void ApplyBindings(QueueDefinition definition)
        {
            if (definition.MessageType == MessageType.Direct)
            {
                Channel!.QueueBind(definition.FullQueueName,definition.Exchange, definition.RouteKey);
            }

            if (definition.MessageType == MessageType.Topic)
            {
                Channel!.QueueBind(definition.FullQueueName,definition.Exchange, definition.RouteKey);
                if (definition.FullWildcardBindings != null)
                    definition.FullWildcardBindings.ToList().ForEach(wildcard => Channel!.QueueBind(definition.FullQueueName,definition.Exchange, wildcard));
            }
        }

        private void ApplyDeadLetterQueueBindings(QueueDefinition definition)
        {
            if (definition.MessageType == MessageType.Direct)
            {
                Channel!.QueueBind($"dlx.{definition.FullQueueName}",definition.Exchange, $"dlx.{definition.RouteKey.Value}");
            }

            if (definition.MessageType == MessageType.Topic)
            {
                Channel!.QueueBind($"dlx.{definition.FullQueueName}",definition.Exchange, definition.RouteKey);
                if (definition.FullWildcardBindings != null)
                    definition.FullWildcardBindings.ToList().ForEach(wildcard => Channel!.QueueBind($"dlx.{definition.FullQueueName}",definition.Exchange, $"dlx.{wildcard.Value}"));
            }
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

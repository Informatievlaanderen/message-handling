using System;
using RabbitMQ.Client;
using Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq.Definitions;

namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    public abstract class Producer<T> : BaseChannel where T : new()
    {
        private readonly int _maxRetry;

        private RouteDefinition RouteDefinition { get; }

        protected virtual void OnPublishMessagesHandler(T[] messages) { }
        protected abstract void OnPublishMessagesExceptionHandler(Exception exception, T[] messages);

        protected Producer(RouteDefinition routeDefinition, IConnection connection, int maxRetry = 5) : base(connection)
        {
            RouteDefinition = routeDefinition;
            _maxRetry = Math.Max(0, maxRetry);
            EnsureExchangeExists(RouteDefinition.Exchange, RouteDefinition.MessageType);
        }

        public void Publish(params T[] messages)
        {
            if (messages.Length == 1)
                Publish(messages[0]);

            if (messages.Length > 1)
                BatchPublish(messages);
        }

        private void Publish(T message)
        {
            try
            {
                ProcessHandler.Retry(() =>
                {
                    EnsureOpenChannel();
                    var buffer = SerializeToUTF8Bytes(message);
                    Channel!.BasicPublish(RouteDefinition.Exchange, RouteDefinition.RouteKey, true, BasicProperties,
                        buffer);
                }, _maxRetry);
                OnPublishMessagesHandler(new[] { message});
            }
            catch (Exception e)
            {
                OnPublishMessagesExceptionHandler(e, new[] { message});
            }
        }

        private void BatchPublish(T[] messages)
        {
            try
            {
                ProcessHandler.Retry(() =>
                {
                    EnsureOpenChannel();
                    var publisher = Channel!.CreateBasicPublishBatch();
                    foreach (var message in messages)
                    {
                        var buffer = SerializeToUTF8Bytes(message);
                        publisher.Add(RouteDefinition.Exchange, RouteDefinition.RouteKey, true, BasicProperties, buffer);
                    }

                    publisher.Publish();
                }, _maxRetry);
                OnPublishMessagesHandler(messages);
            }
            catch (Exception e)
            {
                OnPublishMessagesExceptionHandler(e, messages);
            }
        }
    }
}

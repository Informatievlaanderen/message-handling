namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    using System;
    using RabbitMQ.Client;
    using Polly;
    using Definitions;

    public abstract class BaseProducer<T> : BaseChannel where T : new()
    {
        protected RouteDefinition RouteDefinition { get; }

        protected BaseProducer(
            MessageHandlerContext context,
            MessageType messageType,
            string subscriber,
            string queueName,
            int maxRetry = 5)
            : base(context, maxRetry)
        {
            RouteDefinition = new RouteDefinition(context, messageType, subscriber, queueName);
            EnsureExchangeExists(RouteDefinition.Exchange, RouteDefinition.MessageType);
        }

        public void Publish(params T[] messages)
        {
            if (messages.Length == 1)
                Publish(messages[0]);

            if (messages.Length > 1)
                BatchPublish(messages);
        }

        protected virtual void OnPublishMessagesHandler(T[] messages) { }
        protected abstract void OnPublishMessagesExceptionHandler(Exception exception, T[] messages);

        private void Publish(T message)
        {
            try
            {
                Policy.Handle<Exception>()
                    .Retry()
                    .Execute(() =>
                    {
                        EnsureOpenChannel();
                        var buffer = SerializeToUTF8Bytes(message);
                        Channel!.BasicPublish(RouteDefinition.Exchange, RouteDefinition.RouteKey, true, BasicProperties,
                            buffer);
                    });
                OnPublishMessagesHandler(new[] { message });
            }
            catch (Exception e)
            {
                OnPublishMessagesExceptionHandler(e, new[] { message });
            }
        }

        private void BatchPublish(T[] messages)
        {
            try
            {
                Policy.Handle<Exception>()
                    .Retry()
                    .Execute(() =>
                    {
                        EnsureOpenChannel();
                        var publisher = Channel!.CreateBasicPublishBatch();
                        foreach (var message in messages)
                        {
                            var buffer = SerializeToUTF8Bytes(message);
                            publisher.Add(RouteDefinition.Exchange, RouteDefinition.RouteKey, true, BasicProperties, buffer);
                        }

                        publisher.Publish();
                    });
                OnPublishMessagesHandler(messages);
            }
            catch (Exception e)
            {
                OnPublishMessagesExceptionHandler(e, messages);
            }
        }
    }
}

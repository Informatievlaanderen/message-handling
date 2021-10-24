namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    public abstract class TopicProducer<T> : BaseProducer<T> where T : new()
    {
        protected TopicProducer(MessageHandlerContext context, string queueName,int maxRetry = 5) : base(context, MessageType.Topic, "*", queueName, maxRetry)
        {
            EnsureExchangeExists(RouteDefinition.Exchange, RouteDefinition.MessageType);
        }
    }
}

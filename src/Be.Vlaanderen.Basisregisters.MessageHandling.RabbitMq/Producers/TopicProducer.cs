namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    public abstract class TopicProducer<T> : BaseProducer<T> where T : new()
    {
        protected TopicProducer(MessageHandlerContext context, int maxRetry = 5) : base(context, MessageType.Topic, "*", maxRetry)
        {
            EnsureExchangeExists(RouteDefinition.Exchange, RouteDefinition.MessageType);
        }
    }
}

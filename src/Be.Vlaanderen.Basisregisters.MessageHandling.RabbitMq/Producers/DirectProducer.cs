namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    public abstract class DirectProducer<T> : BaseProducer<T> where T : new()
    {

        protected DirectProducer(MessageHandlerContext context, Module module, string queueName, int maxRetry = 5) : base(context, MessageType.Direct, module, queueName, maxRetry)
        {
            EnsureExchangeExists(RouteDefinition.Exchange, RouteDefinition.MessageType);
        }
    }
}

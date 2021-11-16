namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    public abstract class TopicConsumer<T> : BaseConsumer<T>
    {
        protected TopicConsumer(
            MessageHandlerContext context,
            Module module,
            string queueName)
            : base(
                context,
                MessageType.Topic,
                module,
                queueName)
        { }
    }
}

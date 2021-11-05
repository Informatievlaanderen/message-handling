namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    public abstract class DirectConsumer<T> : BaseConsumer<T>
    {
        protected DirectConsumer(MessageHandlerContext context,
            Module module) : base(context, MessageType.Direct, module)
        {
        }
    }
}

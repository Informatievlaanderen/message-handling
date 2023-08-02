namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    using Confluent.Kafka;

    public sealed class MessageContext
    {
        public MessageKey Key { get; init; }
        public Offset Offset { get; init; }

        public static MessageContext From(ConsumeResult<string, string> consumeResult)
        {
            return new MessageContext
            {
                Key = new MessageKey(consumeResult.Message.Key),
                Offset = new Offset(consumeResult.Offset.Value)
            };
        }
    }
}

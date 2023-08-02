namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    using Confluent.Kafka;

    public sealed class MessageContext<TKey>
    {
        public TKey Key { get; init; }
        public long Offset { get; init; }
    }

    public static class MessageContext
    {
        public static MessageContext<TKey> From<TKey, TValue>(ConsumeResult<TKey, TValue> consumeResult)
        {
            return new MessageContext<TKey>
            {
                Key = consumeResult.Message.Key,
                Offset = consumeResult.Offset.Value
            };
        }
    }
}

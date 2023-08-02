namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    public interface IMessageSerializer<TKey, TValue>
    {
        object Deserialize(TValue value, MessageContext<TKey> context);
        TValue Serialize(object message);
    }
}

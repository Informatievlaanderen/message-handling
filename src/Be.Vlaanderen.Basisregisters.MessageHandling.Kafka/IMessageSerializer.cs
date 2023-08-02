namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    public interface IMessageSerializer<TKey, TValue>
    {
        object Deserialize(TKey key, TValue value);
        TValue Serialize(object message);
    }
}

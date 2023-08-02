namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    public interface IMessageSerializer<TValue>
    {
        object Deserialize(TValue value, MessageContext context);
        TValue Serialize(object message);
    }
}

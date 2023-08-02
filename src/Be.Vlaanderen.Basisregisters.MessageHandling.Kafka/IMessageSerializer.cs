namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    public interface IMessageSerializer<TMessage>
    {
        object Deserialize(TMessage kafkaMessage);
        TMessage Serialize(object message);
    }
}

namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    public sealed record KafkaMessage<TValue>
    {
        public string Key { get; init; }
        public TValue Value { get; init; }
    }
}

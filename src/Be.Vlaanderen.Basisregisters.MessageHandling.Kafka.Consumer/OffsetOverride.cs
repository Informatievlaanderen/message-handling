namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer
{
    public class OffsetOverride
    {
        public string? ConsumerGroupId { get; set; }
        public long Offset { get; set; }
        public bool Configured { get; set; }
    }
}

namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer
{
    using System;

    public class ProcessedMessage
    {
        public string IdempotenceKey { get; set; }
        public DateTimeOffset DateProcessed { get; set; }

        public ProcessedMessage(
            string idempotenceKey,
            DateTimeOffset dateProcessed)
        {
            IdempotenceKey = idempotenceKey;
            DateProcessed = dateProcessed;
        }
    }
}

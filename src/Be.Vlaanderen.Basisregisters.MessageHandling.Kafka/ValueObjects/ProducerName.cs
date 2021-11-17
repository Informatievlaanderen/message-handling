namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    using System;
    using AggregateSource;

    public class ProducerName: StringValueObject<ProducerName>
    {
        public ProducerName(string producerName) : base(producerName)
        {
        }
    }
}

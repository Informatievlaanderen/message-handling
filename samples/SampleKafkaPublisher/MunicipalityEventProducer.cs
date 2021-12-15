namespace SampleKafkaPublisher
{
    using System;
    using Be.Vlaanderen.Basisregisters.MessageHandling.Kafka;
    using Confluent.Kafka;

    public class MunicipalityEventProducer : Producer<string>
    {
        public MunicipalityEventProducer(KafkaContext context)
            : base(context, new TopicName("events"))
        {
        }

        protected override void OnErrorHandler(
            ProduceException<string, string> exception,
            Error error,
            Message<string, string> message)
        {
            throw new NotImplementedException();
        }
    }
}

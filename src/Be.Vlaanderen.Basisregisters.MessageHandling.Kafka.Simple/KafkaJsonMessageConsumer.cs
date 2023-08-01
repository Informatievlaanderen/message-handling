namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    using System;
    using System.Threading.Tasks;
    using Confluent.Kafka;

    public class KafkaJsonMessageConsumer : KafkaConsumer<KafkaJsonMessage, Ignore>
    {
        public KafkaJsonMessageConsumer(KafkaConsumerOptions options)
            : base(options)
        {
        }

        protected override Task HandleMessageAsync(KafkaJsonMessage message)
        {
            return Options.MessageHandler(message.Map() ?? throw new ArgumentException("Kafka message data is null."));
        }
    }
}

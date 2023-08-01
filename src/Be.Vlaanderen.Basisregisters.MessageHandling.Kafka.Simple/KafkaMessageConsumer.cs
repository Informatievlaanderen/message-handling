namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    using Confluent.Kafka;
    using Extensions;

    public class KafkaMessageConsumer<TMessage> : KafkaConsumer<KafkaMessage<TMessage>, string>
    {
        public KafkaMessageConsumer(KafkaConsumerOptions options)
            : base(options)
        {
        }

        protected override KafkaMessage<TMessage>? ConvertConsumeResultToMessage(ConsumeResult<string, string> consumeResult)
        {
            var message = Serializer.Deserialize<TMessage>(consumeResult.Message.Value);
            if (message is null)
            {
                return null;
            }

            return new KafkaMessage<TMessage>
            {
                Key = consumeResult.Message.Key,
                Value = message
            };
        }
    }
}

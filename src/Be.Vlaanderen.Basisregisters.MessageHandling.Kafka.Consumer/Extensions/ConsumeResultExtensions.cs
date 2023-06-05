namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer.Extensions
{
    using System;
    using Confluent.Kafka;

    internal static class ConsumeResultExtensions
    {
        public static long GetMessagesBehind<TFirst, TSecond>(this ConsumeResult<TFirst, TSecond> consumeResult, IConsumer<TFirst, TSecond> consumer)
        {
            var watermarks = consumer.GetWatermarkOffsets(consumeResult.TopicPartition);
            return Math.Max(0, watermarks.High - consumeResult.Offset);
        }
    }
}

namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IIdempotentConsumer<out TConsumerContext>
        where TConsumerContext : ConsumerDbContext<TConsumerContext>
    {
        /// <summary>
        /// Continuously consume from a kafka topic.
        /// </summary>
        /// <param name="messageHandler">The message handler function.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task ConsumeContinuously(Func<object, TConsumerContext, Task> messageHandler, CancellationToken cancellationToken = default);

        /// <summary>
        /// The consumer options.
        /// </summary>
        public ConsumerOptions ConsumerOptions { get; }
    }
}

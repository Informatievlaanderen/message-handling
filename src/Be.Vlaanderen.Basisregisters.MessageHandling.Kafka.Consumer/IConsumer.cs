namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IConsumer
    {
        /// <summary>
        /// Continuously consume from a kafka topic.
        /// </summary>
        /// <param name="messageHandler">The message handler function.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task ConsumeContinuously(Func<object, Task> messageHandler, CancellationToken cancellationToken = default);

        /// <summary>
        /// Continuously consume from a kafka topic.
        /// </summary>
        /// <param name="messageHandler">The message handler function.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task ConsumeContinuously(Func<object, MessageContext, Task> messageHandler, CancellationToken cancellationToken = default);

        /// <summary>
        /// The consumer options
        /// </summary>
        public ConsumerOptions ConsumerOptions { get; }
    }
}

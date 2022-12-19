namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Producer
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for a Kafka Producer
    /// </summary>
    public interface IProducer
    {
        /// <summary>
        /// Produces a message on kafka
        /// </summary>
        /// <param name="key">The key of the message.</param>
        /// <param name="message">The message.</param>
        /// <param name="headers">The optional headers of the message.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A result</returns>
        Task<Result> Produce(
            MessageKey key,
            string message,
            IEnumerable<MessageHeader>? headers = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Produces a message serialized as json on kafka
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="key">The key of the message.</param>
        /// <param name="message">The message.</param>
        /// <param name="headers">The optional headers of the message.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result> ProduceJsonMessage<T>(
            MessageKey key,
            T message,
            IEnumerable<MessageHeader>? headers = null,
            CancellationToken cancellationToken = default)
            where T : class;
    }
}

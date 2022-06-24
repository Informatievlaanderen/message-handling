namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple
{
    using System.Threading;
    using System.Threading.Tasks;

    public static class Sqs
    {
        public static async Task<bool> CopyToQueue<T>(SqsOptions sqsOptions, string queueName, T message, CancellationToken cancellationToken)
            where T : class
        {
            var queueUrl = await SqsQueue.CreateQueueIfNotExists(sqsOptions, queueName, true, cancellationToken);
            await SqsProducer.Produce(sqsOptions, queueUrl, message, string.Empty, cancellationToken);

            return true;
        }
    }
}

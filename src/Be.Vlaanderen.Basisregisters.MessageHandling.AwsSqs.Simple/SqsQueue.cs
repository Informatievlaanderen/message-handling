namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon.SQS;

    public static class SqsQueue
    {
        public static async Task<IEnumerable<string>> ListQueues(SqsOptions options, CancellationToken cancellationToken)
        {
            using var client = new AmazonSQSClient(options.Credentials);
            var response = await client.ListQueuesAsync((string?)null, cancellationToken);
            return response.QueueUrls;
        }

        public static async Task<string> GetQueueUrl(SqsOptions options, string queueName, CancellationToken cancellationToken)
        {
            using var client = new AmazonSQSClient(options.Credentials);
            var response = await client.GetQueueUrlAsync(queueName, cancellationToken);
            return response.QueueUrl;
        }

        public static async Task<string> CreateQueue(SqsOptions options, string queueName, CancellationToken cancellationToken)
        {
            using var client = new AmazonSQSClient(options.Credentials);
            var response = await client.CreateQueueAsync(queueName, cancellationToken);
            return response.QueueUrl;
        }

        public static async Task DeleteQueue(SqsOptions options, string queueUrl, CancellationToken cancellationToken)
        {
            using var client = new AmazonSQSClient(options.Credentials);
            _ = await client.DeleteQueueAsync(queueUrl, cancellationToken);
        }
    }
}

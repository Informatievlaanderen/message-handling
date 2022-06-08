namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon.SQS;
    using Amazon.SQS.Model;

    public static class SqsQueue
    {
        public static async Task<IEnumerable<string>> ListQueues(SqsOptions options, CancellationToken cancellationToken = default)
        {
            using var client = new AmazonSQSClient(options.Credentials, options.RegionEndpoint);
            var response = await client.ListQueuesAsync((string?)null, cancellationToken);
            return response.QueueUrls;
        }

        public static async Task<string> GetQueueUrl(SqsOptions options, string queueName, CancellationToken cancellationToken = default)
        {
            using var client = new AmazonSQSClient(options.Credentials, options.RegionEndpoint);
            var response = await client.GetQueueUrlAsync(queueName, cancellationToken);
            return response.QueueUrl;
        }

        public static async Task<string> CreateQueue(SqsOptions options, string queueName, bool isFifoQueue = false, CancellationToken cancellationToken = default)
        {
            using var client = new AmazonSQSClient(options.Credentials, options.RegionEndpoint);

            if (isFifoQueue)
            {
                queueName += ".fifo";
            }

            var attributes = isFifoQueue
                ? new Dictionary<string, string> { [QueueAttributeName.FifoQueue] = "true", [QueueAttributeName.ContentBasedDeduplication] = "true" }
                : new Dictionary<string, string>();

            var request = new CreateQueueRequest { QueueName = queueName, Attributes = attributes };
            var response = await client.CreateQueueAsync(request, cancellationToken);
            return response.QueueUrl;
        }

        public static async Task DeleteQueue(SqsOptions options, string queueUrl, CancellationToken cancellationToken = default)
        {
            using var client = new AmazonSQSClient(options.Credentials, options.RegionEndpoint);
            _ = await client.DeleteQueueAsync(queueUrl, cancellationToken);
        }
    }
}

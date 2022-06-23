namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon.SQS;
    using Amazon.SQS.Model;

    public static class SqsQueue
    {
        private const string DotFifo = ".fifo";
        
        public static async Task<IEnumerable<string>> ListQueues(SqsOptions options, CancellationToken cancellationToken = default)
        {
            var config = new AmazonSQSConfig { RegionEndpoint = options.RegionEndpoint };
            using var client = new AmazonSQSClient(config);
            var response = await client.ListQueuesAsync((string?)null, cancellationToken);
            return response.QueueUrls;
        }

        public static async Task<string> GetQueueUrl(SqsOptions options, string queueName, CancellationToken cancellationToken = default)
        {
            var config = new AmazonSQSConfig { RegionEndpoint = options.RegionEndpoint };
            using var client = new AmazonSQSClient(config);
            var response = await client.GetQueueUrlAsync(queueName, cancellationToken);
            return response.QueueUrl;
        }

        public static async Task<string> CreateQueue(SqsOptions options, string queueName, bool isFifoQueue = false, CancellationToken cancellationToken = default)
        {
            if (isFifoQueue && !queueName.EndsWith(DotFifo, StringComparison.OrdinalIgnoreCase))
            {
                queueName += DotFifo;
            }

            var config = new AmazonSQSConfig { RegionEndpoint = options.RegionEndpoint };
            using var client = new AmazonSQSClient(config);

            var attributes = isFifoQueue
                ? new Dictionary<string, string> { [QueueAttributeName.FifoQueue] = "true", [QueueAttributeName.ContentBasedDeduplication] = "true" }
                : new Dictionary<string, string>();

            var request = new CreateQueueRequest { QueueName = queueName, Attributes = attributes };
            var response = await client.CreateQueueAsync(request, cancellationToken);
            return response.QueueUrl;
        }

        public static async Task<string> CreateQueueIfNotExists(SqsOptions options, string queueName, bool isFifoQueue = false, CancellationToken cancellationToken = default)
        {
            if (isFifoQueue && !queueName.EndsWith(DotFifo, StringComparison.OrdinalIgnoreCase))
            {
                queueName += DotFifo;
            }

            // check if queue exists
            var queues = await ListQueues(options, cancellationToken);
            var queue = queues.FirstOrDefault(x => x.Split('/').Last().Equals(queueName, StringComparison.OrdinalIgnoreCase));
            return queue ?? await CreateQueue(options, queueName, isFifoQueue, cancellationToken);
        }

        public static async Task DeleteQueue(SqsOptions options, string queueUrl, CancellationToken cancellationToken = default)
        {
            var config = new AmazonSQSConfig { RegionEndpoint = options.RegionEndpoint };
            using var client = new AmazonSQSClient(config);
            _ = await client.DeleteQueueAsync(queueUrl, cancellationToken);
        }
    }
}

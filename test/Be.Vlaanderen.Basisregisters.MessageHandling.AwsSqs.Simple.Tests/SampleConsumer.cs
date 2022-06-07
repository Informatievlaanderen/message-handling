namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon;
    using Amazon.Runtime;

    public static class SampleConsumer
    {
        public static async Task ConsumeMessages(SessionAWSCredentials credentials, RegionEndpoint regionEndpoint, string queueName, CancellationToken cancellationToken)
        {
            var options = new SqsOptions(credentials, regionEndpoint);

            var queueUrl = await SqsQueue.CreateQueue(options, queueName, cancellationToken);

            var result = await SqsConsumer.Consume(options, queueUrl, x => Task.CompletedTask, cancellationToken);
            Console.WriteLine(result.IsSuccess
                ? $"Success: {result.Message}"
                : $"Failed: {result.Error}");
        }
    }
}

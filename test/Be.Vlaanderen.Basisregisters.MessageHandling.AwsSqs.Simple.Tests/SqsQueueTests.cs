using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple.Tests
{
    public class SqsQueueTests
    {
        [Theory(Skip = "Needs theory data")]
        [InlineData("", "", "", -1)]
        public async Task CreateListDeleteQueue(string queueName, string accessKey, string secretKey, int expectedResult)
        {
            var options = new SqsOptions(queueName, accessKey, secretKey);

            await SqsQueue.CreateQueue(options, nameof(SqsQueueTests), CancellationToken.None);
            var topicNames = await SqsQueue.ListQueues(options, default);
            Assert.Contains(topicNames, x => x == nameof(SqsQueueTests));
            string queueUrl = string.Empty;
            try
            {
                queueUrl = await SqsQueue.GetQueueUrl(options, nameof(SqsQueueTests), CancellationToken.None);
            }
            finally
            {
                if (Equals(!string.IsNullOrEmpty(queueUrl)))
                {
                    await SqsQueue.DeleteQueue(options, queueUrl, CancellationToken.None);
                }
            }
        }
    }
}

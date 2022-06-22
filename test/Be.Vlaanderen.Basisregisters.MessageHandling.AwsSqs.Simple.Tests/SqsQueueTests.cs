namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple.Tests
{
    using System.Threading.Tasks;
    using Xunit;

    public class SqsQueueTests
    {
        [Theory(Skip = "Needs theory data")]
        [InlineData("", "", "")]
        public async Task CreateListDeleteQueue(string accessKey, string secretKey, string sessionToken)
        {
            var options = new SqsOptions(null, null);

            await SqsQueue.CreateQueue(options, nameof(SqsQueueTests));
            var topicNames = await SqsQueue.ListQueues(options);
            Assert.Contains(topicNames, x => x == nameof(SqsQueueTests));
            string queueUrl = string.Empty;
            try
            {
                queueUrl = await SqsQueue.GetQueueUrl(options, nameof(SqsQueueTests));
            }
            finally
            {
                if (Equals(!string.IsNullOrEmpty(queueUrl)))
                {
                    await SqsQueue.DeleteQueue(options, queueUrl);
                }
            }
        }
    }
}

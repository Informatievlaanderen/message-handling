namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple.Tests
{
    using System.Threading.Tasks;
    using Amazon;
    using Xunit;

    public class SqsQueueTests
    {
        [Theory(Skip = "Needs theory data")]
        [InlineData("ASIAWYL674VX5KBFULWU", "vChptvDzkl6AKNL6Pwfs2ijkOwNvbtbhMl6UW+s0", "IQoJb3JpZ2luX2VjEF4aCWV1LXdlc3QtMSJIMEYCIQDRaTcxe4nJ2q4TixbPE2yZcwja2yyI3ojL9VVTrgYc2wIhAOjvTYtXW4I2XwCBvypVbN2GVWR0nPTVoQDmcnYE/MOEKt4ECHcQARoMNDY0NjU5NjcwMzgzIgwzf7F6UcLqDjNL53MquwSGcTMh3kPzvQ985pUPh8gVLPCbLexFy+355X259GTEK1oL3cqD8gP+sT5x8d/bQUVbsfI9Dg0Y3kTRMg65XcbchIYKg4e/PpXoEMtotiJI4xR3gNChkabhHHbXl/pHco79kK8Qr4B2ZmT4E9rc6nB6ZNqzhhjEvzrW7M3ORGqA9BZcl1mypXrlSKIQwcXsYmWXFEdT91EoC0BcoFOiMUy8qJszGvyPPWtCZri3taLW8Ip5bQDMwFplJRXyfmN286fKsQAHH46gx4enLMKxQPBeNKZciYneSM0AeltmG/yRPpjaeeA1e1FpdIDSlIiF3fEYFEUnkjWPhJs76htXgmKBxpTxkaLEI5qF1MgYNl/pKtf5BdgJQnyLZDyXxaoDf7uBBVFf2SeD4jkXCBO9FwcJL6x2NrK2vMvDg2EsIvTkZ6gHWRGRHtoL6YNvsOG9O08UBoQLZKCaJlvWGRma1bb4V7GGPnit0TODVH0lXvZdgb2qiP9vcD3NTz/CXNUR49YENXpc4F5kBXumiXfNbvmJAnsp8vno9MKRBqACUK/oleY6N4IRJHZW3e//+8CWKh2zamH7xu/luB6fvK7iMTGsZt9btcUDfQAn0QKMSYpVCzICcoOJsNRA17A4DPw+FpTBhtbaipeXvqaYPiFRoNlaWh/J8SgqzJyW0J/XbmFUnfqPdR8h9eUAgUglxJ48CQ1OtlWddoetsD3fbN3zzfGM4roreHf1ikD71RHEvAv04O+o28YbbhfqMhesMM3m0ZUGOqUBOkrGR8KCSXG22rgLmgjkXcsD7KXChu2hAt40IUvZks+eKMKQ8YapcJxdrYLAoIvdM8kUR/MnG1p4x/v/r2tN3AvDaAuknBhpG7VhWX360dAKfdjMQLzj6PlYZFK3cxk8AzNV9mZBMCYDW45/zlS9TBzcNRtTE93IfcOTJz+0ZAYXEfZ4+gtKOSfzoyFp0mSIfDTtp463+srhwCYdd16vCs/MHVJG")]
        public async Task CreateListDeleteQueue(string accessKey, string secretKey, string sessionToken)
        {
            var options = new SqsOptions(
                accessKey,
                secretKey,
                sessionToken,
                RegionEndpoint.EUWest1);

            await SqsQueue.CreateQueueIfNotExists(options, nameof(SqsQueueTests));
            await SqsQueue.CreateQueueIfNotExists(options, nameof(SqsQueueTests));
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

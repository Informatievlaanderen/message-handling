namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple.Tests
{
    using System;
    using System.Threading.Tasks;
    using Extensions;
    using Newtonsoft.Json;
    using Xunit;

    public class SqsConsumerTests
    {
        [Theory(Skip = "Needs theory data")]
        [InlineData("", "", "")]
        public async Task Consume(string accessKey, string secretKey, string sessionToken)
        {
            var options = new SqsOptions(null, null);

            var queueUrl = await SqsQueue.CreateQueue(options, nameof(SqsConsumerTests));
            try
            {
                const int i = 2;
                var iString = i.ToString();
                await SqsProducer.Produce(options, queueUrl, iString, Guid.NewGuid().ToString("D"));

                var serializer = JsonSerializer.CreateDefault(options.JsonSerializerSettings);

                var result = await SqsConsumer.Consume(options, queueUrl, obj =>
                {
                    var json = serializer.Serialize(obj);
                    var receivedData = serializer.Deserialize<string>(json);
                    Assert.Equal(iString, receivedData);

                    return Task.CompletedTask;
                });

                Assert.True(result.IsSuccess);
            }
            finally
            {
                await SqsQueue.DeleteQueue(options, queueUrl);
            }
        }
    }
}

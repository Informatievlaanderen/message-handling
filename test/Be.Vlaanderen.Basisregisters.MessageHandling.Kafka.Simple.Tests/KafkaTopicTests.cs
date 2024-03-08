namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple.Tests
{
    using System.Threading.Tasks;
    using Xunit;

    public class KafkaTopicTests
    {
        [Theory(Skip = "Needs theory data")]
        [InlineData("", "", "", -1)]
        public async Task CreateListGetDeleteTopic(string bootstrapServers, string userName, string password, int expectedResult)
        {
            var options = new KafkaOptions(bootstrapServers, userName, password);

            await KafkaTopic.CreateTopic(options, nameof(KafkaTopicTests));
            var topicNames = KafkaTopic.ListTopicNames(options);
            Assert.Contains(topicNames, x => x == nameof(KafkaTopicTests));
            try
            {
                int result = int.Parse(await KafkaTopic.GetTopicRetentionMilliseconds(options, nameof(KafkaTopicTests)));
                Assert.Equal(expectedResult, result);
            }
            finally
            {
                await KafkaTopic.DeleteTopic(options, nameof(KafkaTopicTests));
            }
        }
    }
}

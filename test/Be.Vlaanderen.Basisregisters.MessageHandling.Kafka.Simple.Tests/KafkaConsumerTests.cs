using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple.Tests
{
    public class KafkaConsumerTests
    {
        [Theory(Skip = "Needs theory data")]
        [InlineData("", "", "", 2)]
        public async Task ConsumeFromSpecificOffset(string bootstrapServers, string userName, string password, int offset)
        {
            var results = new List<Result<KafkaJsonMessage>>();
            var options = new KafkaOptions(bootstrapServers, userName, password);

            await KafkaTopic.CreateTopic(options, nameof(KafkaConsumerTests));
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    await KafkaProducer.Produce(options, nameof(KafkaConsumerTests), i.ToString(), new { i });
                }

                var result = await KafkaConsumer.Consume(options, nameof(ConsumeFromSpecificOffset), nameof(KafkaConsumerTests), async obj =>
                {
                    await Task.Yield();
                }, offset);

                Assert.True(result.IsSuccess);
                var expectedData = "{\"i\":2}";
                Assert.Equal(expectedData, result.Message?.Data);
            }
            finally
            {
                await KafkaTopic.DeleteTopic(options, nameof(KafkaConsumerTests));
            }
        }
    }
}

namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple.Tests
{
    using System.Threading.Tasks;
    using Xunit;

    public class KafkaConsumerTests
    {
        [Theory(Skip = "Needs theory data")]
        [InlineData("", "", "", 2)]
        public async Task ConsumeFromSpecificOffset(string bootstrapServers, string userName, string password, int offset)
        {
            var producerOptions = new KafkaProducerOptions(bootstrapServers, userName, password, nameof(KafkaConsumerTests));
            var consumerOptions = new KafkaConsumerOptions(bootstrapServers, userName, password, nameof(ConsumeFromSpecificOffset), nameof(KafkaConsumerTests), async obj =>
            {
                await Task.Yield();
            }, offset);

            await KafkaTopic.CreateTopic(producerOptions, nameof(KafkaConsumerTests));
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    await KafkaProducer.Produce(producerOptions, i.ToString(), new { i });
                }

                var result = await KafkaConsumer.Consume(consumerOptions);

                Assert.True(result.IsSuccess);
                const string expectedData = "{\"i\":2}";
                Assert.Equal(expectedData, result.Message?.Data);
            }
            finally
            {
                await KafkaTopic.DeleteTopic(producerOptions, nameof(KafkaConsumerTests));
            }
        }
    }
}

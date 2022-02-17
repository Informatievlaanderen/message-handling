namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple.Tests
{
    using Xunit;

    public class KafkaAuthenticationExtensionsTests
    {
        [Theory]
        [InlineData("None", KafkaAuthentication.None)]
        [InlineData("none", KafkaAuthentication.None)]
        [InlineData("SaslPlainText", KafkaAuthentication.SaslPlainText)]
        [InlineData("saslPlainText", KafkaAuthentication.SaslPlainText)]
        [InlineData("", KafkaAuthentication.None)]
        [InlineData(null, KafkaAuthentication.None)]
        public void FromString(string value, KafkaAuthentication expected)
        {
            Assert.Equal(expected, value.FromString());
        }
    }
}

namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Producer.Tests
{
    using AutoFixture;
    using Confluent.Kafka;
    using FluentAssertions;
    using Xunit;

    public class ProducerOptionsTests
    {
        [Fact]
        public void CreateProducerConfigWithSasl_ThenAllFieldsAreSetCorrectly()
        {
            var fixture = new Fixture();
            var kafkaProducerOptions = new ProducerOptions(
                fixture.Create<BootstrapServers>(),
                fixture.Create<Topic>(),
                fixture.Create<bool>(),
                null)
                .ConfigureSaslAuthentication(fixture.Create<SaslAuthentication>())
                .ConfigureEnableIdempotence(fixture.Create<bool>());

            var result = kafkaProducerOptions.CreateProduceConfig();

            result.BootstrapServers.Should().Be(kafkaProducerOptions.BootstrapServers);
            result.SaslUsername.Should().Be(kafkaProducerOptions.SaslAuthentication!.Value.Username);
            result.SaslPassword.Should().Be(kafkaProducerOptions.SaslAuthentication.Value.Password);
            result.SaslMechanism.Should().Be(SaslMechanism.Plain);
            result.SecurityProtocol.Should().Be(SecurityProtocol.SaslSsl);
        }

        [Fact]
        public void CreateProducerConfigWithoutSasl_ThenAllFieldsAreSetCorrectly()
        {
            var fixture = new Fixture();
            var kafkaProducerOptions = new ProducerOptions(
                    fixture.Create<BootstrapServers>(),
                    fixture.Create<Topic>(),
                    fixture.Create<bool>(),
                    null)
                .ConfigureEnableIdempotence(fixture.Create<bool>());

            var result = kafkaProducerOptions.CreateProduceConfig();

            result.BootstrapServers.Should().Be(kafkaProducerOptions.BootstrapServers);
            result.SaslUsername.Should().BeNullOrEmpty();
            result.SaslPassword.Should().BeNullOrEmpty();
            result.SaslMechanism.Should().BeNull();
            result.SecurityProtocol.Should().BeNull();
        }
    }
}

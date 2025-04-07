namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer.Tests
{
    using AutoFixture;
    using Confluent.Kafka;
    using FluentAssertions;
    using Xunit;

    public class ConsumerOptionsTests
    {
        [Fact]
        public void CreateConsumerConfigWithSasl_ThenAllFieldsAreSetCorrectly()
        {
            var fixture = new Fixture();
            var consumerOptions = new ConsumerOptions(
                    fixture.Create<BootstrapServers>(),
                    fixture.Create<Topic>(),
                    fixture.Create<ConsumerGroupId>(),
                    null)
                .ConfigureSaslAuthentication(fixture.Create<SaslAuthentication>());

            var result = consumerOptions.CreateConsumerConfig();

            result.BootstrapServers.Should().Be(consumerOptions.BootstrapServers);
            result.GroupId.Should().Be(consumerOptions.ConsumerGroupId);
            result.AutoOffsetReset.Should().Be(AutoOffsetReset.Earliest);
            result.EnableAutoCommit.Should().BeFalse();
            result.SaslUsername.Should().Be(consumerOptions.SaslAuthentication!.Value.Username);
            result.SaslPassword.Should().Be(consumerOptions.SaslAuthentication.Value.Password);
            result.SaslMechanism.Should().Be(SaslMechanism.Plain);
            result.SecurityProtocol.Should().Be(SecurityProtocol.SaslSsl);
        }

        [Fact]
        public void CreateConsumerConfigWithoutSasl_ThenAllFieldsAreSetCorrectly()
        {
            var fixture = new Fixture();
            var kafkaProducerOptions = new ConsumerOptions(
                fixture.Create<BootstrapServers>(),
                fixture.Create<Topic>(),
                fixture.Create<ConsumerGroupId>(),
                null);

            var result = kafkaProducerOptions.CreateConsumerConfig();

            result.SaslUsername.Should().BeNullOrEmpty();
            result.SaslPassword.Should().BeNullOrEmpty();
            result.SaslMechanism.Should().BeNull();
            result.SecurityProtocol.Should().BeNull();
        }
    }
}

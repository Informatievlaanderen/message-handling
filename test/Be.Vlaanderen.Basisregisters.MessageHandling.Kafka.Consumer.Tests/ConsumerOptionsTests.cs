namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer.Tests
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFixture;
    using Confluent.Kafka;
    using Extensions;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Moq;
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
            result.SaslUsername.Should().Be(consumerOptions.SaslAuthentication.Value.Username);
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

        [Fact]
        public async Task ConfigureOffsetFor_ThenOffsetIsSetCorrectly()
        {
            var dbFactoryMock = new Mock<IDbContextFactory<FakeDbConsumerContext>>();
            dbFactoryMock.Setup(db => db.CreateDbContextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FakeDbConsumerContextFactory(true).CreateDbContext());

            var fixture = new Fixture();
            var consumerName = fixture.Create<ConsumerName>();
            var offset = fixture.Create<long>();

            var context = await dbFactoryMock.Object.CreateDbContextAsync();
            await context.ConsumerStates.AddAsync(new ConsumerStateItem { Name = consumerName, Offset = offset });
            await context.SaveChangesAsync();

            var consumerOptions = new ConsumerOptions(
                fixture.Create<BootstrapServers>(),
                fixture.Create<Topic>(),
                fixture.Create<ConsumerGroupId>(),
                null);

            await consumerOptions.ConfigureOffsetFor(consumerName, dbFactoryMock.Object);

            consumerOptions.Offset.Should().BeEquivalentTo(new Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Offset(offset));

            consumerOptions.CreateConsumerConfig().BuildConsumer(consumerOptions);
            // cannot assert the offset it's too deep into kafka lib
        }
    }
}

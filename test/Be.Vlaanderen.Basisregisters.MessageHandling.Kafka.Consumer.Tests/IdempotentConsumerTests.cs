namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFixture;
    using Confluent.Kafka;
    using Extensions;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class IdempotentConsumerTests
    {
        private readonly Fixture _fixture;
        private readonly ConsumerOptions _consumerOptions;
        private readonly JsonSerializer _serializer;
        private readonly Mock<IDbContextFactory<FakeDbConsumerContext>> _dbFactoryMock;

        public IdempotentConsumerTests()
        {
            _fixture = new Fixture();

            _consumerOptions = new ConsumerOptions(
                _fixture.Create<BootstrapServers>(),
                _fixture.Create<Topic>(),
                _fixture.Create<ConsumerGroupId>(),
                null);

            _serializer = JsonSerializer.CreateDefault(_consumerOptions.JsonSerializerSettings);

            _dbFactoryMock = new Mock<IDbContextFactory<FakeDbConsumerContext>>();
            _dbFactoryMock.Setup(db => db.CreateDbContextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FakeDbConsumerContextFactory(true).CreateDbContext());
        }

        [Fact]
        public async Task TestMessagesAreConsumedOnceIfIdenticalByHash()
        {
            var expectedFakeMessage = _fixture.Create<FakeMessage>();
            var testMessage = new Message<string, string>
            {
                Key = _fixture.Create<string>(),
                Value = _serializer.Serialize(JsonMessage.Create(expectedFakeMessage, _serializer)),
                Headers = new Headers()
            };

            var consumerMock = new Mock<IConsumer<string, string>>();
            var offset = _fixture.Create<Offset>();
            consumerMock
                .Setup(x => x.Consume(It.IsAny<TimeSpan>()))
                .Returns(new ConsumeResult<string, string> { Message = testMessage, Offset = offset });

            var consumerName = _fixture.Create<string>();
            var consumer = new IdempotentConsumer<FakeDbConsumerContext>(
                new ConsumerName(consumerName),
                _consumerOptions,
                consumerMock.Object,
                _dbFactoryMock.Object,
                new LoggerFactory());

            var messages = new List<FakeMessage>();

            var task = Task.Run(async () =>
            {
                await consumer.ConsumeContinuously(
                    (message, _) =>
                    {
                        var fakeMessage = message as FakeMessage;
                        fakeMessage.Should().NotBeNull();
                        messages.Add(fakeMessage);
                        return Task.CompletedTask;
                    },
                    CancellationToken.None);
            });

            await Task.Delay(2000);

            messages.Should().NotBeEmpty();
            messages.Should().HaveCount(1);
            messages.First().Should().BeEquivalentTo(expectedFakeMessage);

            var context = await _dbFactoryMock.Object.CreateDbContextAsync(CancellationToken.None);
            var processed = await context.ProcessedMessages.FindAsync(testMessage.Value.ToSha512());
            processed.Should().NotBeNull();

            var state = await context.ConsumerStates.FindAsync(new object?[] { consumerName }, CancellationToken.None);
            state.Should().NotBeNull();
            state!.Offset.Should().Be(offset);
        }

        [Fact]
        public async Task TestMessagesAreConsumedOnceIfIdenticalByHeader()
        {
            var expectedFakeMessage = _fixture.Create<FakeMessage>();
            var idempotenceKey = "1";

            var testMessage = new Message<string, string>
            {
                Key = _fixture.Create<string>(),
                Value = _serializer.Serialize(JsonMessage.Create(expectedFakeMessage, _serializer)),
                Headers = new Headers { new MessageHeader(MessageHeader.IdempotenceKey, idempotenceKey) }
            };

            var consumerMock = new Mock<IConsumer<string, string>>();
            var offset = _fixture.Create<Offset>();
            consumerMock
                .Setup(x => x.Consume(It.IsAny<TimeSpan>()))
                .Returns(new ConsumeResult<string, string> { Message = testMessage, Offset = offset });

            var consumerName = _fixture.Create<string>();
            var consumer = new IdempotentConsumer<FakeDbConsumerContext>(
                new ConsumerName(consumerName),
                _consumerOptions,
                consumerMock.Object,
                _dbFactoryMock.Object,
                new LoggerFactory());

            var messages = new List<FakeMessage>();

            var task = Task.Run(async () =>
            {
                await consumer.ConsumeContinuously(
                    (message, _) =>
                    {
                        var fakeMessage = message as FakeMessage;
                        fakeMessage.Should().NotBeNull();
                        messages.Add(fakeMessage);
                        return Task.CompletedTask;
                    },
                    CancellationToken.None);
            });

            await Task.Delay(2000);

            messages.Should().NotBeEmpty();
            messages.Should().HaveCount(1);
            messages.First().Should().BeEquivalentTo(expectedFakeMessage);

            var context = await _dbFactoryMock.Object.CreateDbContextAsync(CancellationToken.None);
            var processed = await context.ProcessedMessages.FindAsync(idempotenceKey);
            processed.Should().NotBeNull();

            var state = await context.ConsumerStates.FindAsync(new object?[] { consumerName }, CancellationToken.None);
            state.Should().NotBeNull();
            state!.Offset.Should().Be(offset);
        }
    }
}

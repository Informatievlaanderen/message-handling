namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFixture;
    using Confluent.Kafka;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;
    using Offset = Offset;

    public class ConsumerTests
    {
        private readonly Fixture _fixture;
        private readonly ConsumerOptions _consumerOptions;
        private readonly JsonSerializer _serializer;

        public ConsumerTests()
        {
            _fixture = new Fixture();

            _consumerOptions = new ConsumerOptions(
                _fixture.Create<BootstrapServers>(),
                _fixture.Create<Topic>(),
                _fixture.Create<ConsumerGroupId>());

            _serializer = JsonSerializer.CreateDefault(_consumerOptions.JsonSerializerSettings);
        }

        [Fact]
        public async Task TestMessagesAreConsumed()
        {
            var expectedFakeMessage = _fixture.Create<FakeMessage>();
            var testMessage = new Message<string, string>
            {
                Key = _fixture.Create<string>(),
                Value = _serializer.Serialize(JsonMessage.Create(expectedFakeMessage, _serializer))
            };

            var consumerMock = new Mock<IConsumer<string, string>>();
            consumerMock
                .Setup(x => x.Consume(It.IsAny<TimeSpan>()))
                .Returns(new ConsumeResult<string, string> { Message = testMessage });

            var cts = new CancellationTokenSource();

            var consumer = new Consumer(_consumerOptions, consumerMock.Object, new LoggerFactory());
            var messages = new BlockingCollection<FakeMessage>();

            await consumer.ConsumeContinuously(
                (message) =>
                {
                    var fakeMessage = message as FakeMessage;
                    fakeMessage.Should().NotBeNull();
                    messages.Add(fakeMessage!, cts.Token);

                    cts.Cancel();

                    return Task.CompletedTask;
                },
                cts.Token);

            messages.Count.Should().BeGreaterOrEqualTo(1);
            messages.TryTake(out var result).Should().BeTrue();
            result.Should().BeEquivalentTo(expectedFakeMessage);
        }

        [Fact]
        public async Task TestMessagesAreConsumedWithMessageContext()
        {
            var expectedFakeMessage = _fixture.Create<FakeMessage>();
            var testMessage = new Message<string, string>
            {
                Key = _fixture.Create<string>(),
                Value = _serializer.Serialize(JsonMessage.Create(expectedFakeMessage, _serializer))
            };

            var consumerMock = new Mock<IConsumer<string, string>>();
            var expectedOffset = _fixture.Create<Confluent.Kafka.Offset>();
            consumerMock
                .Setup(x => x.Consume(It.IsAny<TimeSpan>()))
                .Returns(new ConsumeResult<string, string>
                {
                    Message = testMessage,
                    Offset = expectedOffset
                });

            var cts = new CancellationTokenSource();

            var consumer = new Consumer(_consumerOptions, consumerMock.Object, new LoggerFactory());
            var messages = new BlockingCollection<FakeMessage>();

            await consumer.ConsumeContinuously(
                (message, messageContext) =>
                {
                    var fakeMessage = message as FakeMessage;
                    fakeMessage.Should().NotBeNull();
                    messages.Add(fakeMessage!, cts.Token);

                    messageContext.Offset.Should().Be(new Offset(expectedOffset));
                    messageContext.Key.Should().Be(new MessageKey(testMessage.Key));

                    cts.Cancel();
                    return Task.CompletedTask;
                },
                cts.Token);

            messages.Count.Should().BeGreaterOrEqualTo(1);
            messages.TryTake(out var result).Should().BeTrue();
            result.Should().BeEquivalentTo(expectedFakeMessage);
        }
    }
}

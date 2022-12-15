namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Producer.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFixture;
    using Confluent.Kafka;
    using FluentAssertions;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class ProducerTests
    {
        private readonly Fixture _fixture;

        public ProducerTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task TestProduceSinglePartition()
        {
            var expectedOffset = _fixture.Create<long>();
            var producerMock = new Mock<IProducer<string, string>>();
            producerMock
                .Setup(x => x.ProduceAsync(It.IsAny<TopicPartition>(), It.IsAny<Message<string, string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DeliveryResult<string, string> { Offset = new Offset(expectedOffset) });

            var producerOptions = new ProducerOptions(
                    _fixture.Create<BootstrapServers>(),
                    _fixture.Create<Topic>(),
                    useSinglePartition: true,
                    null)
                .ConfigureEnableIdempotence(_fixture.Create<bool>()); ;

            var producer = new Producer(producerOptions, producerMock.Object);

            var messageKey = _fixture.Create<MessageKey>();
            var message = _fixture.Create<string>();
            var messageHeaders = _fixture.CreateMany<MessageHeader>().ToList();

            var result = await producer.Produce(
                messageKey,
                message,
                messageHeaders,
                CancellationToken.None);

            producerMock.Verify(x => x.ProduceAsync(
                It.Is<TopicPartition>(y => VerifyTopicPartition(y, producerOptions.Topic)),
                It.Is<Message<string, string>>(y => VerifyMessage(y, messageKey, message, messageHeaders)),
                CancellationToken.None));

            result.IsSuccess.Should().BeTrue();
            result.Offset.HasValue.Should().BeTrue();
            result.Offset!.Value.Should().Be(new Kafka.Offset(expectedOffset));
        }

        [Fact]
        public async Task TestProduceMultiPartition()
        {
            var expectedOffset = _fixture.Create<long>();
            var producerMock = new Mock<IProducer<string, string>>();
            producerMock
                .Setup(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<string, string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DeliveryResult<string, string> { Offset = new Offset(expectedOffset) });

            var producerOptions = new ProducerOptions(
                    _fixture.Create<BootstrapServers>(),
                    _fixture.Create<Topic>(),
                    useSinglePartition: false,
                    null)
                .ConfigureEnableIdempotence(_fixture.Create<bool>()); ;

            var producer = new Producer(producerOptions, producerMock.Object);

            var messageKey = _fixture.Create<MessageKey>();
            var message = _fixture.Create<string>();
            var messageHeaders = _fixture.CreateMany<MessageHeader>().ToList();

            var result = await producer.Produce(
                messageKey,
                message,
                messageHeaders,
                CancellationToken.None);

            producerMock.Verify(x => x.ProduceAsync(
                It.Is<string>(topic => topic == producerOptions.Topic),
                It.Is<Message<string, string>>(y => VerifyMessage(y, messageKey, message, messageHeaders)),
                CancellationToken.None));

            result.IsSuccess.Should().BeTrue();
            result.Offset.HasValue.Should().BeTrue();
            result.Offset!.Value.Should().Be(new Kafka.Offset(expectedOffset));
        }

        [Fact]
        public async Task TestProduceJsonMessageSinglePartition()
        {
            var expectedOffset = _fixture.Create<long>();
            var producerMock = new Mock<IProducer<string, string>>();
            producerMock
                .Setup(x => x.ProduceAsync(It.IsAny<TopicPartition>(), It.IsAny<Message<string, string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DeliveryResult<string, string> { Offset = new Offset(expectedOffset) });

            var producerOptions = new ProducerOptions(
                    _fixture.Create<BootstrapServers>(),
                    _fixture.Create<Topic>(),
                    useSinglePartition: true,
                    null)
                .ConfigureEnableIdempotence(_fixture.Create<bool>()); ;

            var producer = new Producer(producerOptions, producerMock.Object);

            var messageKey = _fixture.Create<MessageKey>();
            var message = _fixture.Create<FakeMessage>();
            var messageHeaders = _fixture.CreateMany<MessageHeader>().ToList();

            var result = await producer.ProduceJsonMessage(
                messageKey,
                message,
                messageHeaders,
                CancellationToken.None);

            producerMock.Verify(x => x.ProduceAsync(
                It.Is<TopicPartition>(y => VerifyTopicPartition(y, producerOptions.Topic)),
                It.Is<Message<string, string>>(y => VerifyMessage(y, messageKey, message, messageHeaders)),
                CancellationToken.None));

            result.IsSuccess.Should().BeTrue();
            result.Offset.HasValue.Should().BeTrue();
            result.Offset!.Value.Should().Be(new Kafka.Offset(expectedOffset));
        }

        private static bool VerifyMessage(
            Message<string, string> actualMessage,
            MessageKey messageKey,
            string? message,
            IEnumerable<MessageHeader>? headers)
        {
            var kafkaHeaders = new Headers();
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    kafkaHeaders.Add(header);
                }
            }

            actualMessage.Key.Should().Be(messageKey);
            actualMessage.Value.Should().Be(message);
            actualMessage.Headers.Should().BeEquivalentTo(kafkaHeaders);

            return true;
        }

        private static bool VerifyMessage(
            Message<string, string> actualMessage,
            MessageKey messageKey,
            FakeMessage message,
            IEnumerable<MessageHeader>? headers)
        {
            var kafkaHeaders = new Headers();
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    kafkaHeaders.Add(header);
                }
            }

            var serializer = JsonSerializer.CreateDefault();
            var kafkaJsonMessage = JsonMessage.Create(message, serializer);

            actualMessage.Key.Should().Be(messageKey);
            actualMessage.Value.Should().Be(serializer.Serialize(kafkaJsonMessage));
            actualMessage.Headers.Should().BeEquivalentTo(kafkaHeaders);

            return true;
        }

        private static bool VerifyTopicPartition(TopicPartition actualPartition, Topic expectedTopic)
        {
            actualPartition.Topic.Should().Be(expectedTopic.ToString());
            actualPartition.Partition.Value.Should().Be(0);

            return true;
        }
    }
}

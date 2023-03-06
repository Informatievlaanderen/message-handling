namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Producer
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Newtonsoft.Json;
    using Offset = Offset;

    public sealed class Producer : IProducer, IDisposable
    {
        private readonly ProducerOptions _producerOptions;
        private readonly IProducer<string, string> _producer;

        internal Producer(ProducerOptions producerOptions, IProducer<string, string> producer)
        {
            _producerOptions = producerOptions;
            _producer = producer;
        }

        public Producer(ProducerOptions producerOptions)
         : this(
             producerOptions,
             new ProducerBuilder<string, string>(producerOptions.CreateProduceConfig())
                .SetKeySerializer(Serializers.Utf8)
                .SetValueSerializer(Serializers.Utf8)
                .Build())
        { }

        public async Task<Result> Produce(
            MessageKey key,
            string message,
            IEnumerable<MessageHeader>? headers = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var offset = await ProduceMessage(key, message, headers, cancellationToken);

                return Result.Success(offset);
            }
            catch (ProduceException<Null, string> ex)
            {
                return Result.Failure(ex.Error.Code.ToString(), ex.Error.Reason);
            }
            catch (ProduceException<string, string> ex)
            {
                return Result.Failure(ex.Error.Code.ToString(), ex.Error.Reason);
            }
            catch (OperationCanceledException ex)
            {
                return Result.Failure(ex.Message, ex.Message);
            }
        }

        public async Task<Result> ProduceJsonMessage<T>(
            MessageKey key,
            T message,
            IEnumerable<MessageHeader>? headers = null,
            CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                var serializer = JsonSerializer.CreateDefault(_producerOptions.JsonSerializerSettings);
                var kafkaJsonMessage = JsonMessage.Create(message, serializer);

                var offset = await ProduceMessage(key, serializer.Serialize(kafkaJsonMessage), headers, cancellationToken);

                return Result.Success(offset);
            }
            catch (ProduceException<Null, JsonMessage> ex)
            {
                return Result.Failure(ex.Error.Code.ToString(), ex.Error.Reason);
            }
            catch (ProduceException<string, string> ex)
            {
                return Result.Failure(ex.Error.Code.ToString(), ex.Error.Reason);
            }
            catch (OperationCanceledException ex)
            {
                return Result.Failure(ex.Message, ex.Message);
            }
        }

        private async Task<Offset> ProduceMessage(
            MessageKey key,
            string message,
            IEnumerable<MessageHeader>? headers,
            CancellationToken cancellationToken)
        {
            var kafkaHeaders = new Headers();
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    kafkaHeaders.Add(header);
                }
            }

            if (_producerOptions.UseSinglePartition)
            {
                var result = await _producer.ProduceAsync(
                    new TopicPartition(_producerOptions.Topic, new Partition(0)),
                    new Message<string, string> { Key = key, Value = message, Headers = kafkaHeaders },
                    cancellationToken);

                if (result.Status != PersistenceStatus.Persisted)
                {
                    throw new ProduceException<string, string>(
                        new Error(ErrorCode.Unknown, $"PersistenceStatus was {result.Status}"),
                        result);
                }

                return new Offset(result.Offset.Value);
            }
            else
            {
                var result = await _producer.ProduceAsync(
                        _producerOptions.Topic,
                        new Message<string, string> { Key = key, Value = message, Headers = kafkaHeaders },
                        cancellationToken);

                if (result.Status != PersistenceStatus.Persisted)
                {
                    throw new ProduceException<string, string>(
                        new Error(ErrorCode.Unknown, $"PersistenceStatus was {result.Status}"),
                        result);
                }

                return new Offset(result.Offset.Value);
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _producer.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Producer()
        {
            Dispose(false);
        }
    }
}

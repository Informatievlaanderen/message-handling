namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Confluent.Kafka.SyncOverAsync;

    public abstract class Producer<T> : BaseCluster, IDisposable
    {
        private bool disposed = false;
        private IProducer<string, T>? _producer;
        private readonly TopicName _topicName;
        private readonly Partition _partition;

        protected Producer(KafkaContext context,
            TopicName topicName,
            Partition? partition = null)
            : base(context)
        {
            _topicName = topicName;
            _partition = partition ?? Partition.Any;
        }

        protected abstract void OnErrorHandler(ProduceException<string,T> exception, Error error, Message<string, T> message);

        public async Task PublishAsync(Message<string, T> message,
            CancellationToken ct = default)
        {
            await EnsureTopicExistAsync(_topicName);
            EnsureProducerBuild();
            try
            {
                await _producer!.ProduceAsync(new TopicPartition(_context.ProduceTopics[_topicName].Name!, _partition), message, ct);
            }
            catch (ProduceException<string, T> e)
            {
                OnErrorHandler(e, e.Error, message);
            }
        }

        private void EnsureProducerBuild()
            => _producer ??= new ProducerBuilder<string, T>(_context.ClientConfig)
                .Build();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposeManaged)
        {
            if (disposed) return;

            if (disposeManaged)
            {
                _producer?.Flush(TimeSpan.FromSeconds(10));
                _producer?.Dispose();
            }

            disposed = true;
        }
    }
}

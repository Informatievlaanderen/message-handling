namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    using System;
    using System.Threading;
    using Confluent.Kafka;

    public abstract class Consumer<T> : BaseCluster, IDisposable
    {
        private bool disposed = false;
        private readonly string _topicName;
        private IConsumer<string, T>? _consumer;

        protected Consumer(KafkaContext context, ProducerName producerName, TopicName topicName)
            : base(context)
        {
            if (string.IsNullOrWhiteSpace(producerName))
                throw new ArgumentException("Invalid ProducerName");
            _topicName = $"{context.Environment}-{producerName}-{topicName}";
        }

        protected abstract void OnHandle(ConsumeResult<string, T> consumeResult);

        protected abstract void OnErrorHandler(ConsumeException exception, Error error);

        public void Consume(CancellationToken ct)
        {
            EnsureConsumerBuild();
            _consumer!.Subscribe(_topicName);
            try
            {
                while (true)
                {
                    OnHandle(_consumer.Consume(ct));
                }
            }
            catch (OperationCanceledException e)
            {
            }
            catch (ConsumeException e)
            {
                OnErrorHandler(e, e.Error);
            }
            finally
            {
               Dispose(false);
            }
        }

        private void EnsureConsumerBuild()
            => _consumer ??= new ConsumerBuilder<string, T>(_context.ClientConfig)
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
                _consumer?.Close();
            }

            disposed = true;
        }
    }
}

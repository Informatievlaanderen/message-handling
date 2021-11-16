namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    using System;
    using System.Text;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using Definitions;

    public abstract class BaseConsumer<T> : BaseChannel
    {
        private QueueDefinition? QueueDefinition { get; }

        protected BaseConsumer(
            MessageHandlerContext context,
            MessageType messageType,
            Module module,
            string queueName)
            : base(context)
        {
            QueueDefinition = new QueueDefinition(context, messageType, module, queueName);
        }

        protected abstract T Parse(string message);
        protected abstract void MessageReceive(T message, ulong deliveryTag);
        protected abstract void MessageReceiveException(Exception exception, ulong deliveryTag);

        protected void Ack(ulong deliveryTag)
        {
            Channel!.BasicAck(deliveryTag, false);
        }

        protected void Nack(ulong deliveryTag, bool requeued = true)
        {
            Channel!.BasicNack(deliveryTag, false, requeued);
        }

        protected void Reject(ulong deliveryTag, bool requeued = false)
        {
            Channel!.BasicReject(deliveryTag, requeued);
        }

        public void Watch()
        {
            EnsureQueueExists(QueueDefinition!);
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (model, basicDeliverEventArgs) =>
            {
                try
                {
                    var data = Encoding.UTF8.GetString(basicDeliverEventArgs.Body.ToArray());
                    MessageReceive(Parse(data), basicDeliverEventArgs.DeliveryTag);
                }
                catch (Exception ex)
                {
                    MessageReceiveException(ex, basicDeliverEventArgs.DeliveryTag);
                }
            };
            Channel.BasicConsume(queue: QueueDefinition!.FullQueueName, autoAck: false, consumer: consumer);
        }
    }
}

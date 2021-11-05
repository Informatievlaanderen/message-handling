using System;
using RabbitMQ.Client;
using System.Text;
using Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq.Definitions;
using RabbitMQ.Client.Events;

namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    public abstract class Consumer<T>: BaseChannel
    {
        private QueueDefinition? QueueDefinition { get; }

        protected abstract T Parse(string message);
        protected abstract void MessageReceive(T message, ulong deliveryTag);
        protected abstract void MessageReceiveException(Exception exception, ulong deliveryTag);
        
        protected Consumer(QueueDefinition queueDefinition, IConnection connection) : base(connection)
        {
            QueueDefinition = queueDefinition;
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
    }
}

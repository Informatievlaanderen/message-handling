using System;
using RabbitMQ.Client;

namespace SampleReceiver
{
    public class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqps://XXX:5671"),
                UserName = "admin",
                Password = "123456789abc",
                Port = 5671
            };
            using var connection = factory.CreateConnection();
            using var consumer = new MessageConsumer(connection);
            consumer.Watch();
            Console.WriteLine("Waiting for messages.");
            QuietRun();
        }

        private static void QuietRun()
        {
            bool _quitFlag = false;
            while(!_quitFlag)
            {
                var keyInfo = Console.ReadKey();
                _quitFlag = keyInfo.Key == ConsoleKey.C
                            && keyInfo.Modifiers == ConsoleModifiers.Control;
            }
        }
    }
}

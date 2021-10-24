using System;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace SamplePublisher
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
            Console.Write("Publisher");
            while (true)
            {
                #region Generate Messages
                Console.Write("\nDo you want to send a message? (Y/N): ");
                var keypress = Console.ReadKey();
                if(keypress.Key != ConsoleKey.Y && keypress.Key != ConsoleKey.N) continue;
                if (keypress.Key == ConsoleKey.N) break;
                Console.WriteLine("\nHow many messages do you want to send?");
                var input = Console.ReadLine()!;
                int count = int.Parse(input);
                List<Message> messages = new();
                for (int i = 0; i < count; i++)
                {
                    messages.Add(new Message()
                    {
                        Name = $"Message No.: {i}",
                        Content = "Lorum Ipsum",
                        Version = i
                    });
                }
                #endregion

                using var messagePublisher = new MessagePublisher(connection);
                messagePublisher.Publish(messages.ToArray());
            }
        }
    }
}

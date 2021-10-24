using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SamplePublisher.Messages;
using SamplePublisher.Publishers;

namespace SamplePublisher
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly MessagePublisher _publisher;
        private readonly BarMessagePublisher _barPublisher;
        private readonly StreetNameMessagePublisher _streetnamePublisher;
        private readonly AddressMessagePublisher _addressPublisher;

        public App(MessagePublisher publisher, BarMessagePublisher barPublisher, StreetNameMessagePublisher streetnamePublisher, AddressMessagePublisher addressPublisher, ILoggerFactory loggerFactory)
        {
            _publisher = publisher;
            _barPublisher = barPublisher;
            _streetnamePublisher = streetnamePublisher;
            _addressPublisher = addressPublisher;
            _logger = loggerFactory.CreateLogger<App>();
        }

        public async Task Run()
        {
            Console.Write("Publisher");
            bool isDirect = false;
            bool isStreetName = false;
            while (true)
            {
                #region Generate Messages
                Console.Write("\nDo you want to send a message? (Y/N): ");
                var keypress = Console.ReadKey();
                if (keypress.Key != ConsoleKey.Y && keypress.Key != ConsoleKey.N) continue;
                if (keypress.Key == ConsoleKey.N) break;

                Console.Write("\nDirect message or topic message? (D/T): ");
                var keypress1 = Console.ReadKey();
                if (keypress1.Key != ConsoleKey.D && keypress1.Key != ConsoleKey.T) continue;

                if (keypress1.Key == ConsoleKey.D)
                {
                    isDirect = true;
                    Console.Write("\nstreetname-registry or address-registry? (S/A): ");

                    var keypress2 = Console.ReadKey();
                    if (keypress2.Key != ConsoleKey.S && keypress2.Key != ConsoleKey.A) continue;
                    if (keypress2.Key == ConsoleKey.S)
                        isStreetName = true;
                    else
                        isStreetName = false;
                }
                else
                {
                    isDirect = false;
                }

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

                if (isDirect)
                {
                    if (isStreetName)
                    {
                        _streetnamePublisher.Publish(messages.ToArray());
                    }
                    else
                    {
                        _addressPublisher.Publish(messages.ToArray());
                    }
                }
                else
                {
                    _publisher.Publish(messages.ToArray());
                }
            }
        }
    }
}

namespace SampleReceiver
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Messages;
    using Consumers;

    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly MessageConsumer _consumer;
        private readonly BarMessageConsumer _barConsumer;
        private readonly DirectMessageConsumer _directConsumer;

        public App(MessageConsumer consumer, BarMessageConsumer barConsumer, DirectMessageConsumer directMessageConsumer, ILoggerFactory loggerFactory)
        {
            _consumer = consumer;
            _barConsumer = barConsumer;
            _directConsumer = directMessageConsumer;
            _logger = loggerFactory.CreateLogger<App>();
        }

        public async Task Run()
        {
            Console.WriteLine("streetname-registry started");
            _consumer.Watch();
            Console.WriteLine("Waiting for topic messages from municipality-registry");
            _directConsumer.Watch();
            Console.WriteLine("Waiting for topic bar messages from municipality-registry");
            _directConsumer.Watch();
            Console.WriteLine("Waiting for direct messages from municipality-registry");
            QuietRun();
        }

        private void QuietRun()
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

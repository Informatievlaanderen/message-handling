namespace SampleKafkaPublisher.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using Be.Vlaanderen.Basisregisters.MessageHandling.Kafka;

    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                AppBuilder.Build<App>(args).Run().Wait();
                return 0;
            }
            catch(Exception ex)
            {
                return 1;
            }
        }

    }
}

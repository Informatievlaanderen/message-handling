namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    using System;

    public static class ProcessHandler
    {
        public static void Retry(Action method, int numberOfTries)
        {
            try
            {
                method();
            }
            catch (Exception)
            {
                if (numberOfTries > 0)
                {
                    --numberOfTries;
                    Retry(method, numberOfTries);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}

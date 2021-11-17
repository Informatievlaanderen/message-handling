namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    using System;
    using AggregateSource;

    public class Environment : StringValueObject<Environment>
    {
        public Environment(string environment) : base(environment)
        {
            if (string.IsNullOrWhiteSpace(environment))
                throw new ArgumentException("environment is missing");
        }
    }
}

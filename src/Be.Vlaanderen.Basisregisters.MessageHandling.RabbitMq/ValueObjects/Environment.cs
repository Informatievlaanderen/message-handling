namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    using AggregateSource;

    public class Environment : StringValueObject<Module>
    {
        public Environment(string environment) : base(environment)
        {
        }
    }
}

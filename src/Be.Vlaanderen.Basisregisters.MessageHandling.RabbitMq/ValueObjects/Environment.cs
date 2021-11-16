namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    //TODO: remove aggregate source dependency
    using AggregateSource;

    public class Environment : StringValueObject<Module>
    {
        public Environment(string environment) : base(environment)
        { }
    }
}

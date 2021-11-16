namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    using AggregateSource;

    public class Module : StringValueObject<Module>
    {
        public Module(string module) : base(module)
        { }
    }
}

namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    public class Exchange
    {
        public string Value { get; }

        private Exchange(string value) => Value = value;

        public override string ToString() => Value;

        public static Exchange Create(MessageType type, Environment environment, Module module)
            => new($"{type}.{environment}.{module}");

        public static implicit operator string(Exchange exchange) => exchange.Value;
    }
}

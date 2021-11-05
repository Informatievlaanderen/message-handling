namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    public class Exchange
    {
        private Exchange(string value) => Value = value;

        public string Value { get; }

        private Exchange() {}

        public static Exchange Create(MessageType type, Environment environment, Module module)
            => new($"{type}.{environment}.{module}");

        public override string ToString() => Value;
        public static implicit operator string(Exchange exchange) => exchange.Value;
    }
}

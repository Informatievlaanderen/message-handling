namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    public class Exchange
    {
        private Exchange() {}
        private Exchange(string value) => Value = value;

        public string Value { get; }

        public static Exchange Create(MessageType type, Environment environment, Module module)
            => new($"basisregisters.{environment.Value}.{module.Value}.{type}");

        public override string ToString() => Value;
        public static implicit operator string(Exchange exchange) => exchange.Value;
    }
}

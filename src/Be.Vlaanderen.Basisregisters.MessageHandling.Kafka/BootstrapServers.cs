namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    public readonly struct BootstrapServers
    {
        private readonly string _value;

        public BootstrapServers(string value)
        {
            _value = value;
        }

        public static implicit operator string(BootstrapServers bootstrapServers) => bootstrapServers._value;
        public static explicit operator BootstrapServers(string bootstrapServers) => new BootstrapServers(bootstrapServers);

        public override string ToString() => $"{_value}";
    }
}

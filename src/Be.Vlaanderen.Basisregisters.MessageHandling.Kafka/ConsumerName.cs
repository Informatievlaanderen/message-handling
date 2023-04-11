namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    public readonly struct ConsumerName
    {
        private readonly string _value;

        public ConsumerName(string value)
        {
            _value = value;
        }

        public static implicit operator string(ConsumerName consumerName) => consumerName._value;
        public static explicit operator ConsumerName(string consumerName) => new ConsumerName(consumerName);

        public override string ToString() => _value;
    }
}

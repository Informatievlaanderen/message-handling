namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    public readonly struct ConsumerGroupId
    {
        private readonly string _value;

        public ConsumerGroupId(string value)
        {
            _value = value;
        }

        public static implicit operator string(ConsumerGroupId topic) => topic._value;
        public static explicit operator ConsumerGroupId(string topic) => new ConsumerGroupId(topic);

        public override string ToString() => $"{_value}";
    }

    public readonly struct ConsumerName
    {
        private readonly string _value;

        public ConsumerName(string value)
        {
            _value = value;
        }

        public static implicit operator string(ConsumerName consumerName) => consumerName._value;
        public static explicit operator ConsumerName(string consumerName) => new ConsumerName(consumerName);

        public override string ToString() => $"{_value}";
    }
}

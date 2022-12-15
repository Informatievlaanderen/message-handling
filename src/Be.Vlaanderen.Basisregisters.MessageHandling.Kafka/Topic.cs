namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    public readonly struct Topic
    {
        private readonly string _value;

        public Topic(string value)
        {
            _value = value;
        }

        public static implicit operator string(Topic topic) => topic._value;
        public static explicit operator Topic(string topic) => new Topic(topic);

        public override string ToString() => $"{_value}";
    }
}

namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    public readonly struct MessageKey
    {
        private readonly string _value;

        public MessageKey(string value)
        {
            _value = value;
        }

        public static implicit operator string(MessageKey messageKey) => messageKey._value;
        public static explicit operator MessageKey(string messageKey) => new MessageKey(messageKey);

        public override string ToString() => $"{_value}";
    }
}

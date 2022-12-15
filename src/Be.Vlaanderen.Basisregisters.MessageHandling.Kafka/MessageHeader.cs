namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    using System.Text;
    using Confluent.Kafka;

    public readonly struct MessageHeader
    {
        public const string IdempotenceKey = "IdempotenceKey";

        public string Key { get; }
        public string Value { get; }

        public MessageHeader(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public static implicit operator Header(MessageHeader header)
            => new Header(header.Key, Encoding.UTF8.GetBytes(header.Value));

        public static explicit operator MessageHeader(Header messageKey)
            => new MessageHeader(messageKey.Key, Encoding.UTF8.GetString(messageKey.GetValueBytes()));
    }
}

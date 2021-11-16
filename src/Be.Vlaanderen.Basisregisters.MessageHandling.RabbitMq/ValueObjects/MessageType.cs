namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    using System;

    public readonly struct MessageType
    {
        public static MessageType Direct = new("direct");
        public static MessageType Topic = new("topic");

        public string Value { get; }

        private MessageType(string value) => Value = value;

        public override string ToString() => Value;

        public static MessageType? Parse(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            if (name != Direct.Value &&
                name != Topic.Value)
                throw new NotImplementedException($"Cannot parse {name} to {nameof(MessageType)}");

            return new MessageType(name);
        }

        public static implicit operator string(MessageType name) => name.Value;
    }
}

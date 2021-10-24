using System;

namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    public readonly struct QueueName
    {
        private readonly string _value;

        public QueueName(string name)
        {
            if (name.Contains('*') || name.Contains('#'))
                throw new ArgumentException($"Error: '*' and '#' are reserved characters.");
            _value = name;
        }

        public string Value => _value;
        public override string ToString() => Value;
        public static implicit operator string(QueueName name) => name.Value;
    }
}

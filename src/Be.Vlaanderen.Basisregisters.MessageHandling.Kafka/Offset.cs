namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    public readonly struct Offset
    {
        private readonly long _value;

        public Offset(long value)
        {
            _value = value;
        }

        public static implicit operator long(Offset offset) => offset._value;
        public static explicit operator Offset(long offset) => new Offset(offset);

        public override string ToString() => $"{_value}";
    }
}

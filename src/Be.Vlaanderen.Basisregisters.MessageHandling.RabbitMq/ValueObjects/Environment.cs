using System;

namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    public readonly struct Environment
    {
        public static Environment Development = new("dev");
        public static Environment Staging = new("stg");
        public static Environment Test = new("tst");
        public static Environment Production = new("prd");

        public string? Value { get; }

        private Environment(string value) => Value = value;

        public static Environment? Parse(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            if (name != Development.Value &&
                name != Staging.Value &&
                name != Test.Value &&
                name != Production.Value)
                throw new NotImplementedException($"Cannot parse {name} to {nameof(Environment)}");

            return new Environment(name);
        }

        public override string ToString() => Value!;

        public static implicit operator string(Environment name) => name.Value!;
    }
}

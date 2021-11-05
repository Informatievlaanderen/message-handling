namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    using System;
    public sealed class RouteKey
    {
        public string Value { get; }

        private RouteKey() {}
        private RouteKey(string value) => Value = value;

        public static RouteKey Create(MessageType type, Environment environment, Module module, string name)
        {
            if (type != MessageType.Topic && (name.Contains('*') || name.Contains('#')))
                throw new ArgumentException("Wildcards are only supported in topic exchanges");

            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Invalid name");

            var route =  $"{type}.{environment}.{module}.{name}";

            if (route.Length > 255)
                throw new ArgumentException("Route key is limited to max 255 characters");

            return new RouteKey(route);
        }

        public static implicit operator string(RouteKey route) => route.Value;
    }
}

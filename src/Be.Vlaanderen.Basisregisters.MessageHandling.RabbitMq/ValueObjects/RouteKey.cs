namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    using System;
    public sealed class RouteKey
    {
        public string Value { get; }

        private RouteKey(string value) => Value = value;

        public static RouteKey Create(
            MessageType type,
            Environment environment,
            Module publisher,
            string subscriber,
            string queueName)
        {
            if (type != MessageType.Topic && (subscriber.Contains('*') || subscriber.Contains('#')))
                throw new ArgumentException("Wildcards are only supported in topic exchanges");

            if (string.IsNullOrWhiteSpace(subscriber) && string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentException("Invalid name");

            var route = $"{type}.{environment}.{publisher}.{subscriber}.{queueName}";

            if (route.Length > 255)
                throw new ArgumentException("Route key is limited to max 255 characters");

            return new RouteKey(route);
        }

        public static implicit operator string(RouteKey route) => route.Value;
    }
}

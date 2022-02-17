using System;

namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    public static class KafkaAuthenticationExtensions
    {
        public static KafkaAuthentication FromString(this string value) => (Enum.TryParse(typeof(KafkaAuthentication), value, true, out var result))
            ? (KafkaAuthentication)result!
            : KafkaAuthentication.None;
    }
}

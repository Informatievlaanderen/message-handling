namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Extensions;
    using Newtonsoft.Json;

    public class KafkaJsonMessage
    {
        public string Type { get; set; }
        public string Data { get; set; }

        public KafkaJsonMessage(string type, string data)
        {
            Type = type;
            Data = data;
        }

        public T? Map<T>()
        {
            var assembly = typeof(T).Assembly;
            var type = assembly.GetType(Type);

            return (T?)JsonConvert.DeserializeObject(Data, type!);
        }

        public static KafkaJsonMessage Create<T>([DisallowNull] T message, JsonSerializer serializer)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            return new KafkaJsonMessage(message.GetType().FullName!, serializer.Serialize(message));
        }
    }
}

using System.Linq;

namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
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

        public object? Map()
        {
            var assembly = GetAssemblyNameContainingType(Type);
            var type = assembly?.GetType(Type);

            return JsonConvert.DeserializeObject(Data, type!);
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

        private static Assembly? GetAssemblyNameContainingType(string typeName) => AppDomain.CurrentDomain.GetAssemblies()
            .Select(x => new
            {
                Assembly = x,
                Type = x.GetType(typeName, false, true)
            })
            .Where(x => x.Type != null)
            .Select(x => x.Assembly)
            .FirstOrDefault();
    }
}

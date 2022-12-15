namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;

    /// <summary>
    /// The JsonMessage on kafka.
    /// </summary>
    public sealed class JsonMessage
    {
        /// <summary>
        /// The type of the message as fully qualified name.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The data of the message serialized as Json.
        /// </summary>
        public string Data { get; set; }

        public JsonMessage(string type, string data)
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

        public static JsonMessage Create<T>([DisallowNull] T message, JsonSerializer serializer)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            return new JsonMessage(message.GetType().FullName!, serializer.Serialize(message));
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

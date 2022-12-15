namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using Newtonsoft.Json;

    public static class JsonSerializerExtensions
    {
        public static string Serialize(
            this JsonSerializer jsonSerializer,
            object value)
        {
            var stringWriter = new StringWriter(new StringBuilder(256), CultureInfo.InvariantCulture);
            using (var jsonTextWriter = new JsonTextWriter(stringWriter))
            {
                jsonTextWriter.Formatting = jsonSerializer.Formatting;
                jsonSerializer.Serialize(jsonTextWriter, value, value.GetType());
            }

            return stringWriter.ToString();
        }

        public static T? Deserialize<T>(this JsonSerializer serializer, string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            using var stringReader = new StringReader(message);
            using var jsonReader = new JsonTextReader(stringReader);

            return serializer.Deserialize<T>(jsonReader);
        }
    }
}

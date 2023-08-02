namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    using System;
    using Newtonsoft.Json;

    public class JsonMessageSerializer : IMessageSerializer<string>
    {
        private readonly JsonSerializer _serializer;

        public JsonMessageSerializer(JsonSerializerSettings jsonSerializerSettings)
        {
            _serializer = JsonSerializer.CreateDefault(jsonSerializerSettings);
        }

        public object Deserialize(string value, MessageContext context)
        {
            var kafkaJsonMessage = _serializer.Deserialize<JsonMessage>(value)
                                   ?? throw new ArgumentException("Kafka json message is null.");
            return kafkaJsonMessage.Map()
                   ?? throw new ArgumentException("Kafka message data is null.");
        }

        public string Serialize(object message)
        {
            var kafkaJsonMessage = JsonMessage.Create(message, _serializer);
            return _serializer.Serialize(kafkaJsonMessage);
        }
    }
}

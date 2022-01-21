using Confluent.Kafka;
using System;

namespace SampleSimpleKafkaMessages
{
    public interface IKafkaSerializable<T> : ISerializer<T>, IDeserializer<T>
    {
        byte[] Serialize(T data, SerializationContext context);
        T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context);
    }
}

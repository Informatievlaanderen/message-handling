namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Producer
{
    using System.Net;
    using Confluent.Kafka;
    using Extensions;
    using Newtonsoft.Json;

    public class ProducerOptions
    {
        public Topic Topic { get; }
        public bool UseSinglePartition { get; }

        public BootstrapServers BootstrapServers { get; }
        public SaslAuthentication? SaslAuthentication { get; private set; }
        public JsonSerializerSettings JsonSerializerSettings { get; }

        public bool EnableIdempotence { get; private set; } = false;

        public ProducerOptions(
            BootstrapServers bootstrapServers,
            Topic topic,
            bool useSinglePartition = true,
            JsonSerializerSettings? jsonSerializerSettings = null)
        {
            BootstrapServers = bootstrapServers;
            Topic = topic;
            UseSinglePartition = useSinglePartition;
            JsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
        }

        public ProducerOptions ConfigureSaslAuthentication(SaslAuthentication saslAuthentication)
        {
            SaslAuthentication = saslAuthentication;
            return this;
        }

        public ProducerOptions ConfigureEnableIdempotence(bool enableIdempotence = true)
        {
            EnableIdempotence = enableIdempotence;
            return this;
        }

        internal ProducerConfig CreateProduceConfig()
        {
            return new ProducerConfig
            {
                BootstrapServers = BootstrapServers,
                ClientId = Dns.GetHostName(),
                EnableIdempotence = EnableIdempotence,

                // https://thecloudblog.net/post/building-reliable-kafka-producers-and-consumers-in-net/
                EnableDeliveryReports = true,
                // Receive acknowledgement from all sync replicas
                Acks = Acks.All,

            }.WithAuthentication(this);
        }
    }
}

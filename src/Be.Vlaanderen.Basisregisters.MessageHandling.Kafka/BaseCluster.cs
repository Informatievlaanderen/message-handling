namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Confluent.Kafka.Admin;

    public abstract class BaseCluster
    {
        private readonly AdminClientBuilder _adminClientBuilder;
        protected readonly KafkaContext _context;

        protected BaseCluster(KafkaContext context)
        {
            _context = context;
            _adminClientBuilder = new AdminClientBuilder(_context!.ClientConfig);

        }

        protected async Task EnsureTopicExistAsync(TopicName topicName)
        {
            if (!_context.ProduceTopics.Any())
                throw new Exception("No ProduceTopics are defined");

            if (!_context.ProduceTopics.ContainsKey(topicName))
                throw new Exception("Topic not found");

            try
            {
                using var adminClient = _adminClientBuilder.Build();
                await adminClient.CreateTopicsAsync(new[] {_context.ProduceTopics[topicName]});
            }
            catch (CreateTopicsException e)
            {
                foreach (var topicCreateReport in e.Results)
                {
                    if (topicCreateReport.Error.Code != ErrorCode.TopicAlreadyExists)
                        throw;
                }
            }
        }
    }
}

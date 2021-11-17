namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    using System;
    using AggregateSource;

    public class TopicName : StringValueObject<TopicName>
    {
        public TopicName(string topicName) : base(topicName)
        {
            if (string.IsNullOrWhiteSpace(topicName))
                throw new ArgumentException("Invalid TopicName");
        }
    }
}

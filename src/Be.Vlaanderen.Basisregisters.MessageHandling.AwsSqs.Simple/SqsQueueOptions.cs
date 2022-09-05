namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple
{
    public record SqsQueueOptions(string MessageGroupId = "", string MessageDeduplicationId = "", bool CreateQueueIfNotExists = false);
}

namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon.SQS.Model;
    using Extensions;
    using Newtonsoft.Json;

    public static class SqsProducer
    {
        public static async Task<Result<T>> Produce<T>(
            SqsOptions options,
            string queueUrl,
            T message,
            string groupId = "",
            string deduplicationId = "",
            CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                if (!Uri.IsWellFormedUriString(queueUrl, UriKind.Absolute))
                {
                    throw new ArgumentException("queueUrl is not a valid uri", nameof(queueUrl));
                }

                var serializer = JsonSerializer.CreateDefault(options.JsonSerializerSettings);
                var sqsJsonMessage = SqsJsonMessage.Create(message, serializer);
                var json = serializer.Serialize(sqsJsonMessage);

                using var client = options.CreateSqsClient();
                var request = new SendMessageRequest(queueUrl, json);
                if (!string.IsNullOrEmpty(groupId))
                {
                    request.MessageGroupId = groupId;
                }

                if (!string.IsNullOrEmpty(deduplicationId))
                {
                    request.MessageDeduplicationId = deduplicationId;
                }

                _ = await client.SendMessageAsync(request, cancellationToken);
                
                return Result<T>.Success(message);
            }
            catch (TaskCanceledException ex)
            {
                return Result<T>.Failure(ex.Message, ex.Message);
            }
            catch (OperationCanceledException)
            {
                return Result<T>.Success(default);
            }
        }
    }
}

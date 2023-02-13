namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon.SQS;
    using Amazon.SQS.Model;
    using Extensions;
    using Newtonsoft.Json;

    public static class SqsConsumer
    {
        public static async Task<Result<SqsJsonMessage>> Consume(
            SqsOptions options,
            string queueUrl,
            Func<object, Task> messageHandler,
            CancellationToken cancellationToken = default)
        {
            var serializer = JsonSerializer.CreateDefault(options.JsonSerializerSettings);

            var config = new AmazonSQSConfig { RegionEndpoint = options.RegionEndpoint };
            using var client = new AmazonSQSClient(config);

            const string messageGroupId = "MessageGroupId";
            var sqsJsonMessage = new SqsJsonMessage();
            var request = new ReceiveMessageRequest(queueUrl) { AttributeNames = new List<string> { messageGroupId } };
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var response = await client.ReceiveMessageAsync(request, cancellationToken);
                    if (response.Messages.Count == 0)
                    {
                        break;
                    }

                    var message = response.Messages[0];
                    //var groupId = message.Attributes[messageGroupId];

                    sqsJsonMessage = serializer.Deserialize<SqsJsonMessage>(message.Body) ?? throw new ArgumentException("SQS json message is null.");
                    var messageData = sqsJsonMessage.Map(serializer) ?? throw new ArgumentException("SQS message data is null.");

                    await messageHandler(messageData);

                    await client.DeleteMessageAsync(queueUrl, message.ReceiptHandle, cancellationToken);
                }

                return Result<SqsJsonMessage>.Success(sqsJsonMessage);
            }
            catch (TaskCanceledException ex)
            {
                return Result<SqsJsonMessage>.Failure(ex.Message, ex.Message);
            }
            catch (OperationCanceledException)
            {
                return Result<SqsJsonMessage>.Success(sqsJsonMessage);
            }
        }
    }
}

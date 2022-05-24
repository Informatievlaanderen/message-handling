namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple
{
    using System;
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

            var message = new Message();
            var sqsJsonMessage = new SqsJsonMessage();
            using var client = new AmazonSQSClient(options.Credentials);
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var response = await client.ReceiveMessageAsync(queueUrl, cancellationToken);
                    if (response.Messages.Count > 0)
                    {
                        continue;
                    }

                    message = response.Messages[0];
                    sqsJsonMessage = serializer.Deserialize<SqsJsonMessage>(message.Body) ?? throw new ArgumentException("SQS json message is null.");
                    var messageData = sqsJsonMessage.Map() ?? throw new ArgumentException("SQS message data is null.");

                    await messageHandler(messageData);
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
            finally
            {
                await client.DeleteMessageAsync(queueUrl, message.ReceiptHandle, cancellationToken);
            }
        }
    }
}

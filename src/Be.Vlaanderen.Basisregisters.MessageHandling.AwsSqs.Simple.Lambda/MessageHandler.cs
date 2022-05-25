using System.Threading.Tasks;

namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple.Lambda
{
    public class MessageHandler : IMessageHandler
    {
        public async Task HandleMessage(object? messageData)
        {
            await Task.Yield();

            if (messageData is not null)
            {
                await Task.Yield();
            }
        }
    }
}

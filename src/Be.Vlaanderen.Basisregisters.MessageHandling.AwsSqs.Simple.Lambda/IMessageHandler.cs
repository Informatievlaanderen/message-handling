using System.Threading.Tasks;

namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple.Lambda
{
    public interface IMessageHandler
    {
        Task HandleMessage(object? messageData);
    }
}

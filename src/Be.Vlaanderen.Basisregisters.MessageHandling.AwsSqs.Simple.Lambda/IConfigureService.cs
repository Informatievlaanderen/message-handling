namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple.Lambda
{
    using Microsoft.Extensions.Configuration;

    public interface IConfigureService
    {
        IConfiguration Configuration { get; }
    }
}

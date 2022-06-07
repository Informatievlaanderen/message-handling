namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple.Lambda
{
    public interface IEnvironmentService
    {
        string EnvironmentName { get; set; }
    }
}

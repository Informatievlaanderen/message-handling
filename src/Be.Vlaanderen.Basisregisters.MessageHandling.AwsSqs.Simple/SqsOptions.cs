namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple
{
    using Amazon;
    using Amazon.Runtime;
    using Newtonsoft.Json;

    public class SqsOptions 
    {
        public SessionAWSCredentials Credentials { get; }
        public RegionEndpoint RegionEndpoint { get; }
        public JsonSerializerSettings JsonSerializerSettings { get; }

        public SqsOptions(SessionAWSCredentials credentials, RegionEndpoint regionEndpoint, JsonSerializerSettings? jsonSerializerSettings = null)
        {
            Credentials = credentials;
            RegionEndpoint = regionEndpoint;
            JsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
        }

        public SqsOptions(string accessKey, string secretKey, string sessionToken, RegionEndpoint regionEndpoint, JsonSerializerSettings? jsonSerializerSettings = null)
            : this(new SessionAWSCredentials(accessKey, secretKey, sessionToken), regionEndpoint, jsonSerializerSettings)
        { }
    }
}

using Amazon.SQS;

namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple
{
    using Amazon;
    using Amazon.Runtime;
    using Newtonsoft.Json;

    public class SqsOptions
    {
        public AWSCredentials? Credentials { get; set; }
        public RegionEndpoint RegionEndpoint { get; }
        public JsonSerializerSettings JsonSerializerSettings { get; }

        public SqsOptions(JsonSerializerSettings jsonSerializerSettings)
            : this(null, jsonSerializerSettings)
        {
        }

        public SqsOptions(RegionEndpoint? regionEndpoint = null, JsonSerializerSettings? jsonSerializerSettings = null)
        {
            RegionEndpoint = regionEndpoint ?? RegionEndpoint.EUWest1;
            JsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
        }

        public SqsOptions(AWSCredentials credentials, RegionEndpoint regionEndpoint)
            : this(regionEndpoint)
        {
            Credentials = credentials;
        }

        public SqsOptions(string accessKey, string secretKey, RegionEndpoint regionEndpoint)
            : this(new BasicAWSCredentials(accessKey, secretKey), regionEndpoint)
        { }

        public SqsOptions(string accessKey, string secretKey, string sessionToken, RegionEndpoint regionEndpoint)
            : this(new SessionAWSCredentials(accessKey, secretKey, sessionToken), regionEndpoint)
        { }

        public virtual AmazonSQSClient CreateSqsClient()
        {
            var config = new AmazonSQSConfig { RegionEndpoint = RegionEndpoint };
            return Credentials != null ? new AmazonSQSClient(Credentials, config) : new AmazonSQSClient(config);
        }
    }
}

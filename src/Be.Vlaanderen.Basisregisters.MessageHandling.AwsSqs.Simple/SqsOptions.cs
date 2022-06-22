namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple
{
    using Amazon;
    //using Amazon.Runtime;
    using Newtonsoft.Json;

    public class SqsOptions
    {
        //public AWSCredentials? Credentials { get; }
        public RegionEndpoint RegionEndpoint { get; }
        public JsonSerializerSettings JsonSerializerSettings { get; }

        public SqsOptions(RegionEndpoint? regionEndpoint, JsonSerializerSettings? jsonSerializerSettings)
        {
            RegionEndpoint = regionEndpoint ?? RegionEndpoint.EUWest1;
            JsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
        }

        //private SqsOptions(AWSCredentials credentials, RegionEndpoint? regionEndpoint, JsonSerializerSettings? jsonSerializerSettings)
        //    : this(regionEndpoint, jsonSerializerSettings)
        //{
        //    Credentials = credentials;
        //}

        //public SqsOptions(BasicAWSCredentials credentials, RegionEndpoint? regionEndpoint = null, JsonSerializerSettings? jsonSerializerSettings = null)
        //    : this(credentials as AWSCredentials, regionEndpoint, jsonSerializerSettings)
        //{ }

        //public SqsOptions(string accessKey, string secretKey, RegionEndpoint? regionEndpoint = null, JsonSerializerSettings? jsonSerializerSettings = null)
        //    : this(new BasicAWSCredentials(accessKey, secretKey), regionEndpoint, jsonSerializerSettings)
        //{ }

        //public SqsOptions(SessionAWSCredentials credentials, RegionEndpoint? regionEndpoint = null, JsonSerializerSettings? jsonSerializerSettings = null)
        //    : this(credentials as AWSCredentials, regionEndpoint, jsonSerializerSettings)
        //{ }

        //public SqsOptions(string accessKey, string secretKey, string sessionToken, RegionEndpoint? regionEndpoint = null, JsonSerializerSettings? jsonSerializerSettings = null)
        //    : this(new SessionAWSCredentials(accessKey, secretKey, sessionToken), regionEndpoint, jsonSerializerSettings)
        //{ }
    }
}

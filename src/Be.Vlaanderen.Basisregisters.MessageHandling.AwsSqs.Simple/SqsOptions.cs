namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple
{
    using Amazon.Runtime;
    using Newtonsoft.Json;

    public class SqsOptions 
    {
        public AWSCredentials Credentials { get; }
        public JsonSerializerSettings JsonSerializerSettings { get; }

        public SqsOptions(BasicAWSCredentials credentials, JsonSerializerSettings? jsonSerializerSettings = null)
        {
            Credentials = credentials;
            JsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
        }

        public SqsOptions(string accessKey, string secretKey, JsonSerializerSettings? jsonSerializerSettings = null)
            : this(new BasicAWSCredentials(accessKey, secretKey), jsonSerializerSettings)
        { }

        public SqsOptions(SessionAWSCredentials credentials, JsonSerializerSettings? jsonSerializerSettings = null)
        {
            Credentials = credentials;
            JsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
        }

        public SqsOptions(string accessKey, string secretKey, string sessionToken, JsonSerializerSettings? jsonSerializerSettings = null)
            : this(new SessionAWSCredentials(accessKey, secretKey, sessionToken), jsonSerializerSettings)
        { }
    }
}

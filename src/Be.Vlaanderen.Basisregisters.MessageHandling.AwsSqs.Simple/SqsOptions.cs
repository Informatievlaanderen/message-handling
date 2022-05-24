namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple
{
    using Amazon.Runtime;
    using Newtonsoft.Json;

    public class SqsOptions 
    {
        public BasicAWSCredentials Credentials { get; set; }
        public JsonSerializerSettings JsonSerializerSettings { get; }

        public SqsOptions(string queueUrl, BasicAWSCredentials credentials, JsonSerializerSettings? jsonSerializerSettings = null)
        {
            Credentials = credentials;
            JsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
        }

        public SqsOptions(string queueUrl, string accessKey, string secretKey, JsonSerializerSettings? jsonSerializerSettings = null)
        {
            Credentials = new BasicAWSCredentials(accessKey, secretKey);
            JsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
        }
    }
}

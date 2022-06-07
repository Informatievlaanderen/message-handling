namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple
{
    using Amazon;
    using Amazon.Runtime;
    using Newtonsoft.Json;

    public class SqsOptions 
    {
        public SessionAWSCredentials Credentials { get; }
        public RegionEndpoint RegionEndpoint { get; }
        public bool IsFifoQueue { get; }
        public string GroupId{ get; }
        public JsonSerializerSettings JsonSerializerSettings { get; }

        public SqsOptions(SessionAWSCredentials credentials, RegionEndpoint regionEndpoint, bool isFifoQueue = false, string groupId = "", JsonSerializerSettings? jsonSerializerSettings = null)
        {
            Credentials = credentials;
            RegionEndpoint = regionEndpoint;
            IsFifoQueue = isFifoQueue;
            GroupId = groupId;
            JsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
        }

        public SqsOptions(string accessKey, string secretKey, string sessionToken, RegionEndpoint regionEndpoint, bool isFifoQueue = false, string groupId = "", JsonSerializerSettings? jsonSerializerSettings = null)
            : this(new SessionAWSCredentials(accessKey, secretKey, sessionToken), regionEndpoint, isFifoQueue, groupId, jsonSerializerSettings)
        { }
    }
}

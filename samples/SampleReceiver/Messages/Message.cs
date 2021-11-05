namespace SampleReceiver.Messages
{
    public class Message
    {
        public string? Name { get; set; }
        public int Version { get; set; }
        public string? Content { get; set; }

        public override string ToString() => $"Version: {Version} | Name: {Name} | Content: {Content}";
    }
}

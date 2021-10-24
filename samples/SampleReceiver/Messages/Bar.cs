namespace SampleReceiver.Messages
{
    public class Bar
    {
        public string? Name { get; set; }
        public int Version { get; set; }
        public string? Content { get; set; }
        public string Foo { get; set; }

        public override string ToString() => $"Version: {Version} | Name: {Name} | Content: {Content} | Foo: {Foo}";
    }
}

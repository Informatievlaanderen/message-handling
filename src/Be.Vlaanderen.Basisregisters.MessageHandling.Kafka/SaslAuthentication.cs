namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    public readonly struct SaslAuthentication
    {
        public string Username { get; }
        public string Password { get; }

        public SaslAuthentication(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public override string ToString() => $"Username: {Username} / Password: {Password}";
    }
}

namespace HowTo.WebApi.Logging
{
    public class LogEntryProxy
    {
        public string? TransactionId { get; set; }

        public string? Username { get; set; }

        public Dictionary<string, string>? Tags { get; set; }

        public object? Data { get; set; }
    }
}

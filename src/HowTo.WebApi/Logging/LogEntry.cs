using MongoDB.Bson;

namespace HowTo.WebApi.Logging
{
    public class LogEntry
    {
        public ObjectId _id { get; set; }

        public DateTimeOffset LogTime { get; set; }

        public string? MethodName { get; set; }

        public string? ClassName { get; set; }

        public string? TransactionId { get; set; }

        public string? LogLevel { get; set; }

        public string? Username { get; set; }

        public string? ApplicationName { get; set; }

        public Dictionary<string, string>? Tags { get; set; }

        public object? Data { get; set; }
    }
}

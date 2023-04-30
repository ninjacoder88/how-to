using MongoDB.Driver;
using System.Runtime.CompilerServices;

namespace HowTo.WebApi.Logging
{
    public interface ILogRepository
    {
        Task LogInformationAsync(object thisInstance, Action<LogEntryBuilder> logBuilder, [CallerMemberName] string? methodName = null);

        Task LogErrorAsync(object thisInstance, Action<LogEntryBuilder> logBuilder, [CallerMemberName] string? methodName = null);
    }

    internal class LogRepository : ILogRepository
    {
        public LogRepository(string connectionString, string applicationName, bool throwExceptions = false)
        {
            _logEntries = new MongoClient(connectionString).GetDatabase("Logs").GetCollection<LogEntry>("LogEntries");
            _applicationName = applicationName;
            _throwExceptions = throwExceptions;
        }

        public async Task LogInformationAsync(object thisInstance, Action<LogEntryBuilder> logBuilder, [CallerMemberName] string? methodName = null)
        {
            LogEntryBuilder builder = new LogEntryBuilder();
            logBuilder(builder);

            LogEntryProxy proxy = builder.Build();

            await LogAsync(thisInstance, "Information", proxy, methodName);
        }

        public async Task LogErrorAsync(object thisInstance, Action<LogEntryBuilder> logBuilder, [CallerMemberName] string? methodName = null)
        {
            LogEntryBuilder builder = new LogEntryBuilder();
            logBuilder(builder);

            LogEntryProxy proxy = builder.Build();

            await LogAsync(thisInstance, "Error", proxy, methodName);
        }

        private async Task LogAsync(object thisInstance, string logLevel, LogEntryProxy proxy, string? methodName)
        {
            try
            {
                await _logEntries.InsertOneAsync(new LogEntry
                {
                    ApplicationName = _applicationName,
                    ClassName = thisInstance?.GetType().FullName,
                    Data = proxy.Data,
                    LogLevel = logLevel,
                    LogTime = DateTimeOffset.Now,
                    MethodName = methodName,
                    TransactionId = proxy.TransactionId,
                    Username = proxy.Username
                });
            }
            catch
            {
                if(_throwExceptions)
                    throw;
            }
        }

        private readonly IMongoCollection<LogEntry> _logEntries;
        private readonly string _applicationName;
        private readonly bool _throwExceptions;
    }
}

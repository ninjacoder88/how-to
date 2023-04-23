namespace HowTo.WebApi.Logging
{
    public class LogEntryBuilder
    {
        public LogEntryBuilder()
        {
            _proxy = new LogEntryProxy();
        }

        public LogEntryBuilder AddTransactionId(string transactionId)
        {
            _proxy.TransactionId = transactionId;
            return this;
        }

        public LogEntryBuilder AddUsername(string username)
        {
            _proxy.Username = username;
            return this;
        }

        public LogEntryBuilder AddData(object data)
        {
            _proxy.Data = data;
            return this;
        }

        public LogEntryBuilder AddTag(string key, string value)
        {
            if (_proxy.Tags == null)
                _proxy.Tags = new Dictionary<string, string>();

            if (!_proxy.Tags.ContainsKey(key))
                _proxy.Tags.Add(key, value);

            return this;
        }

        internal LogEntryProxy Build()
        {
            return _proxy;
        }

        private LogEntryProxy _proxy;
    }
}

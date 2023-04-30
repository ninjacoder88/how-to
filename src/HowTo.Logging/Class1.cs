using Microsoft.AspNetCore.Mvc;

namespace HowTo.Logging
{
    public interface ILogBuilder
    {
        ILogBuilder Start();

        ILogBuilder End();

        ILogBuilder Log(string message);

        ILogBuilder AddRequestParameters(params object[] requestParameters);

        ILogBuilder AddUser(string username);

        ILogBuilder AddResponse(object response);
    }

    public class LogBuilder
    {
        public Func<ILogBuilder, IActionResult> DoIt(Func<IActionResult> action)
        {
            return null;
        }
    }
}
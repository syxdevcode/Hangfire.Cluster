using Hangfire.Samples.Framework.Logging;
using System;

namespace Hangfire.Samples.Framework
{
    public class NullLogger : ILog
    {
        public bool Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters)
        {
            return true;
        }
    }
}

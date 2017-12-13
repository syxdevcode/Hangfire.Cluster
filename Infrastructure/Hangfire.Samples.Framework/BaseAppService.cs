using Hangfire.Samples.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.Samples.Framework
{
    public abstract class BaseAppService : IAppService
    {
        public virtual ILog Logger { get; set; } = new NullLogger();
    }
}

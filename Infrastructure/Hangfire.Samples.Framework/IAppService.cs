using Hangfire.Samples.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.Samples.Framework
{
    public interface IAppService : IDependency
    {
        ILog Logger { get; set; }
    }
}

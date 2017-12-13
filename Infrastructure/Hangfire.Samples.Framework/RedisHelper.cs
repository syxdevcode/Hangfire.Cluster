using StackExchange.Redis;
using System;

namespace Hangfire.Samples.Framework
{
    public class RedisHelper
    {
        private static Lazy<RedisHelper> _instance = new Lazy<RedisHelper>(() => new RedisHelper());
        public static RedisHelper Instance => _instance.Value;

        private RedisHelper()
        {
            Connection = ConnectionMultiplexer.Connect("192.168.0.107:6379,allowAdmin=true,password=toor,defaultdatabase=5");
            Database = Connection.GetDatabase();
        }
        public ConnectionMultiplexer Connection { get; private set; }
        public IDatabase Database { get; private set; }
    }
}

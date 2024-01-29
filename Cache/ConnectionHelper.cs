using StackExchange.Redis;

namespace BookApi
{

    /// <summary>
    /// Helper class for managing Redis connection.
    /// </summary>
    public class ConnectionHelper
    {
        static ConnectionHelper()
        {
            ConnectionHelper.lazyConn = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(ConfigurationManager.AppSetting["RedisCloudUrl"]!);
            });
        }

        private static Lazy<ConnectionMultiplexer> lazyConn;

        /// <summary>
        /// Gets the Redis connection.
        /// </summary>
        public static ConnectionMultiplexer connection
        {
            get
            {
                return lazyConn.Value;
            }
        }
    }
}

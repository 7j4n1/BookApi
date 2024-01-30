using StackExchange.Redis;
using Newtonsoft.Json;
using BookApi.Cache;

namespace BookApi
{
    public class CacheService : ICacheService
    {
        private IDatabase? _redisDB;
        public CacheService()
        {
            try
            {
                ConfigureRedis();
            }
            catch (System.Exception)
            {
                
                throw new Exception("error establishing connection to redis server.");
            }
            
            
        }

        /// <summary>
        /// Configures the Redis connection and initializes the Redis database.
        /// </summary>
        public void ConfigureRedis(){
            _redisDB = ConnectionHelper.connection.GetDatabase();
        }
        /// <summary>
        /// Retrieves data from the cache based on the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the data to retrieve.</typeparam>
        /// <param name="key">The key used to identify the data in the cache.</param>
        /// <returns>The retrieved data, or the default value of type T if the data is not found in the cache.</returns>
        public T? GetData<T>(string key)
        {
            var value = _redisDB?.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value!);
            }

            return default;
        }

        /// <summary>
        /// Removes data from the cache using the specified key.
        /// </summary>
        /// <param name="key">The key of the data to remove from the cache.</param>
        /// <returns><c>true</c> if the data was successfully removed; otherwise, <c>false</c>.</returns>
        public object RemoveData(string key)
        {
            bool? _isKeyExist = _redisDB?.KeyExists(key!);
            if (_isKeyExist.HasValue && _isKeyExist.Value)
            {
                return _redisDB.KeyDelete(key);
            }
            return false;
        }

        /// <summary>
        /// Sets the data in the cache with the specified key and expiration time.
        /// </summary>
        /// <typeparam name="T">The type of the value to be stored in the cache.</typeparam>
        /// <param name="key">The key to associate with the value in the cache.</param>
        /// <param name="value">The value to be stored in the cache.</param>
        /// <param name="expirationTime">The expiration time for the cached value.</param>
        /// <returns><c>true</c> if the data is successfully set in the cache; otherwise, <c>false</c>.</returns>
        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);

            var isSet = _redisDB.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);

            return isSet;
        }
    }
}

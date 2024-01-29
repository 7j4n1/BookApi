namespace BookApi.Cache
{
    public interface ICacheService
    {
        /// <summary>
        /// Gets the data of type T from the cache using the specified key.
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve from the cache.</typeparam>
        /// <param name="key">The key used to identify the data in the cache.</param>
        /// <returns>The data of type T retrieved from the cache.</returns>
        T? GetData<T>(string key);

        /// <summary>
        /// Sets the data in the cache with the specified key and expiration time.
        /// </summary>
        /// <typeparam name="T">The type of the value to be stored in the cache.</typeparam>
        /// <param name="key">The key associated with the data.</param>
        /// <param name="value">The value to be stored in the cache.</param>
        /// <param name="expirationTime">The expiration time for the cached data.</param>
        /// <returns><c>true</c> if the data is successfully stored in the cache; otherwise, <c>false</c>.</returns>
        bool SetData<T>(string key, T value, DateTimeOffset expirationTime);

        /// <summary>
        /// Removes the data associated with the specified key from the cache.
        /// </summary>
        /// <param name="key">The key of the data to be removed.</param>
        /// <returns>The removed data.</returns>
        object RemoveData(string key);
    }
}

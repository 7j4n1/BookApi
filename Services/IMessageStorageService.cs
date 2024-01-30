namespace BookApi
{
    /// <summary>
    /// Represents a service for storing and retrieving messages.
    /// </summary>
    public interface IMessageStorageService
    {
        /// <summary>
        /// Stores a message asynchronously.
        /// </summary>
        /// <param name="message">The message to store.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task StoreMessageAsync(string message);

        /// <summary>
        /// Retrieves a message asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation that returns the retrieved message.</returns>
        Task<string> GetMessageAsync();
    }
}

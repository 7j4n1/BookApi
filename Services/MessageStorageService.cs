
namespace BookApi
{
 
    /// <summary>
    /// Represents a service for storing and retrieving messages.
    /// </summary>
    public class MessageStorageService: IMessageStorageService
    {
        private string _message;

        /// <summary>
        /// Retrieves the stored message asynchronously.
        /// </summary>
        /// <returns>The stored message.</returns>
        public Task<string> GetMessageAsync()
        {
            return Task.FromResult(_message);
        }

        /// <summary>
        /// Stores a message asynchronously.
        /// </summary>
        /// <param name="message">The message to be stored.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task StoreMessageAsync(string message)
        {
            _message = message;
            return Task.CompletedTask;
        }
    }
}

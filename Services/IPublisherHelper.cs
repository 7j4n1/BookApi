namespace BookApi
{
    public interface IPublisherHelper
    {
        /// <summary>
        /// Checks if an publisher exists by their ID.
        /// </summary>
        /// <param name="id">The ID of the publisher.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the publisher exists.</returns>
        Task <bool> IsPublisherExistById(int id);

    }
}

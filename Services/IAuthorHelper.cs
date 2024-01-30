using BookApi.Models;

namespace BookApi.Services
{
    public interface IAuthorHelper
    {

        /// <summary>
        /// Checks if an author exists by their ID.
        /// </summary>
        /// <param name="id">The ID of the author.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the author exists.</returns>
        Task <bool> IsAuthorExistById(int id);

    }
}

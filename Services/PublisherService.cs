using BookApi.DataDTO;
using BookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Services
{
    public class PublisherService(LibraryDBContext libraryDB) : IPublisherService, IPublisherHelper
    {
        /// <summary>
        /// Represents a service for managing publishers in the library.
        /// </summary>
        private readonly LibraryDBContext _libraryDB = libraryDB;

        /// <summary>
        /// Adds a new publisher to the database.
        /// </summary>
        /// <param name="Publisher">The publisher to be added.</param>
        /// <returns>The added publisher if successful, otherwise null.</returns>
        public async Task<Publisher?> AddPublisher(PublisherDTO Publisher)
        {
            // since the ID is unique
            // Check if the first name and last name does exist

            var existingPublisher = await _libraryDB.Publishers.FirstOrDefaultAsync(x => EF.Functions.Like(x.Name, Publisher.Name));
            
            if (existingPublisher is not null)
            {
                // throw new Exception("An Publisher with the same Name already exists.");
                return null;
            }

            // if name does not exist, Add the Publisher Info to DB
            var newPublisher = new Publisher() {
                Name = Publisher.Name
            };
            
            var result = await _libraryDB.Publishers.AddAsync(newPublisher);

            await _libraryDB.SaveChangesAsync();

            return result.Entity;
        }

        /// <summary>
        /// Deletes a publisher from the database.
        /// </summary>
        /// <param name="id">The ID of the publisher to delete.</param>
        /// <returns>True if the publisher was deleted successfully, false otherwise.</returns>
        public async Task<bool> DeletePublisher(int id)
        {
            // check if the id exists
            var existingPublisher = await _libraryDB.Publishers.FindAsync(id);

            if (existingPublisher is not null)
            {
                // remove the Publisher info
                _libraryDB.Publishers.Remove(existingPublisher);

                var result = await _libraryDB.SaveChangesAsync();
                // return true if the Publisher entity were deleted successfully
                return result > 0;
            }
            return false;
        }

        /// <summary>
        /// Retrieves a publisher by its ID.
        /// </summary>
        /// <param name="id">The ID of the publisher to retrieve.</param>
        /// <returns>The publisher with the specified ID, or null if not found.</returns>
        public async Task<Publisher?> GetPublisherById(int id)
        {
            // fetch Publisher with the specified ID
            return await _libraryDB.Publishers.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Retrieves a list of publishers along with their associated books.
        /// </summary>
        /// <returns>An asynchronous task that represents the operation. The task result contains the list of publishers.</returns>
        public async Task<IEnumerable<Publisher>> GetPublishers()
        {
            // Return the list of Publishers, along with their associated books
            Task<List<Publisher>> task = _libraryDB.Publishers.Include(x => x.Books).ToListAsync();
            return await task;
        }

        /// <summary>
        /// Checks if a publisher exists by its ID.
        /// </summary>
        /// <param name="id">The ID of the publisher.</param>
        /// <returns>True if the publisher exists, false otherwise.</returns>
        public async Task<bool> IsPublisherExistById(int id)
        {
            // check if the id exists
            var existingPublisher = await _libraryDB.Publishers.FindAsync(id);

            return existingPublisher is not null;
        }

        /// <summary>
        /// Updates a Publisher with the specified ID.
        /// </summary>
        /// <param name="Publisher">The updated Publisher information.</param>
        /// <param name="id">The ID of the Publisher to update.</param>
        /// <returns>The updated Publisher if found, otherwise null.</returns>
        public async Task<Publisher?> UpdatePublisher(PublisherDTO Publisher, int id)
        {
            // Find the Publisher with the specified ID
            var existingPublisher = await _libraryDB.Publishers.FirstOrDefaultAsync(x => x.Id == id);
            // If the Publisher was not found, return null
            if (existingPublisher is null)
            {
                return null;
            }

            // Update the Publisher's properties
            existingPublisher.Name = string.IsNullOrEmpty(Publisher.Name) ? existingPublisher.Name : Publisher.Name;
            
            _libraryDB.Entry(existingPublisher).State = EntityState.Modified;

            // Save the changes to the database
            await _libraryDB.SaveChangesAsync();

            return existingPublisher;
        }
    }
}

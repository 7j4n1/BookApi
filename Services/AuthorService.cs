using System.Security.Cryptography.X509Certificates;
using BookApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Services
{
    public class AuthorService(LibraryDBContext libraryDB) : IAuthorService
    {
        private readonly LibraryDBContext _libraryDB = libraryDB;

        /// <summary>
        /// Adds a new author to the database.
        /// </summary>
        /// <param name="author">The author object to be added.</param>
        /// <returns>The added author object.</returns>
        public async Task <Author?> AddAuthor(Author author)
        {
            // check if the author id already exists
            var existingAuthor = await _libraryDB.Authors.FindAsync(author.Id);

            if (existingAuthor is not null)
            {
                // throw new Exception("An author with the same ID already exists.");
                return null;
            }
            // if id does not exist, Add the author Info to DB
            var newAuthor = new Author() {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Bio = author.Bio
            };
            
           var result = await _libraryDB.Authors.AddAsync(newAuthor);

            await _libraryDB.SaveChangesAsync();

            return result.Entity;
        }

        /// <summary>
        /// Deletes an author from the database.
        /// </summary>
        /// <param name="id">The ID of the author to delete.</param>
        /// <returns>True if the author was deleted successfully, otherwise false.</returns>
        public async Task<bool> DeleteAuthor(int id)
        {
            // check if the id exists
            var existingAuthor = await _libraryDB.Authors.FindAsync(id);

            if (existingAuthor is null)
            {
                return false;
            }
            // remove the author info
            _libraryDB.Authors.Remove(existingAuthor);

            var result = await _libraryDB.SaveChangesAsync();
            // return true if the author entity were deleted successfully
            return result > 0;
        }

        /// <summary>
        /// Retrieves an author by their ID.
        /// </summary>
        /// <param name="id">The ID of the author to retrieve.</param>
        /// <returns>The author with the specified ID, or null if not found.</returns>
        public async Task<Author?> GetAuthorById(int id)
        {
            // fetch Author with the specified ID
            return await _libraryDB.Authors.FindAsync(id);

        }

        /// <summary>
        /// Retrieves a list of authors from the database.
        /// </summary>
        /// <returns>An asynchronous operation that returns a collection of Author objects.</returns>
        public async Task<IEnumerable<Author>> GetAuthors()
        {
            // Return the list of authors, along with their associated books
            Task<List<Author>> task = _libraryDB.Authors.Include(x => x.Books).ToListAsync();
            return await task;
        }

        // Update the author with the specified ID
        public async Task<Author?> UpdateAuthor(Author author)
        {
            // Find the author with the specified ID
            var existingAuthor = await _libraryDB.Authors.FindAsync(author.Id);
            // If the author was not found, return null
            if (existingAuthor is null)
            {
                return null;
            }

            // Update the author's properties
            existingAuthor.FirstName = author.FirstName;
            existingAuthor.LastName = author.LastName;
            existingAuthor.Bio = author.Bio;

            var result = _libraryDB.Authors.Update(existingAuthor);

            // Save the changes to the database
            await _libraryDB.SaveChangesAsync();

            return result.Entity;


        }
    }
}

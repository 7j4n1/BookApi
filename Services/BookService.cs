using BookApi.DataDTO;
using BookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Services
{
    public class BookService(LibraryDBContext libraryDB) : IBookService
    {
        private readonly LibraryDBContext _libraryDB = libraryDB;
        /// <summary>
        /// Adds a new book to the database.
        /// </summary>
        /// <param name="Book">The book to be added.</param>
        /// <returns>The added book if successful, otherwise null.</returns>
        public async Task<Book?> AddBook(BookDTO Book)
        {
            // since the ID is unique
            // Check if the Title does exist

            var existingBook = await _libraryDB.Books.FirstOrDefaultAsync(x => EF.Functions.Like(x.Title, Book.Title));
            
            if (existingBook is not null)
            {
                // throw new Exception("A book with the Title already exists.");
                return null;
            }


            // if name does not exist, Add the book Info to DB
            var newBook = new Book() {
                Title = Book.Title,
                ISBN =  Book.ISBN,
                AuthorId = Book.AuthorId,
                TotalPages = Book.TotalPages,
                PublisherId = Book.PublisherId,
                PublishedDate = Book.PublishedDate

            };
            
           var result = await _libraryDB.Books.AddAsync(newBook);

            await _libraryDB.SaveChangesAsync();

            return result.Entity;
        }

        /// <summary>
        /// Deletes a book from the database.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <returns>True if the book was deleted successfully, false otherwise.</returns>
        public async Task<bool> DeleteBook(int id)
        {
            // check if the id exists
            var existingBook = await _libraryDB.Books.FindAsync(id);

            if (existingBook is null)
            {
                return false;
            }
            // remove the book info
            _libraryDB.Books.Remove(existingBook);

            var result = await _libraryDB.SaveChangesAsync();
            // return true if the book entity were deleted successfully
            return result > 0;
        }

        /// <summary>
        /// Retrieves a book from the database based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the book to retrieve.</param>
        /// <returns>The book with the specified ID, or null if not found.</returns>
        public async Task<Book?> GetBookById(int id)
        {
            // fetch Book with the specified ID
            return await _libraryDB.Books.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Retrieves a list of books along with their associated authors and publishers.
        /// </summary>
        /// <returns>An asynchronous task that represents the operation. The task result contains the list of books.</returns>
        public async Task<IEnumerable<Book>> GetBooks()
        {
            // Return the list of Books, along with their associated books
            Task<List<Book>> task = _libraryDB.Books.Include(x => x.Author).Include(x => x.Publisher).ToListAsync();
            return await task;
        }

        /// <summary>
        /// Updates a book in the database with the specified ID.
        /// </summary>
        /// <param name="Book">The updated book information.</param>
        /// <param name="id">The ID of the book to update.</param>
        /// <returns>The updated book if found, otherwise null.</returns>
        public async Task<Book?> UpdateBook(BookDTO Book, int id)
        {
            // Find the Book with the specified ID
            var existingBook = await _libraryDB.Books.FirstOrDefaultAsync(x => x.Id == id);
            // If the Book was not found, return null
            if (existingBook is null)
            {
                return null;
            }

            // Update the Book's properties
            existingBook.Title = string.IsNullOrEmpty(Book.Title) ? existingBook.Title : Book.Title;
            existingBook.ISBN =  string.IsNullOrEmpty(Book.ISBN) ? existingBook.ISBN : Book.ISBN;
            existingBook.AuthorId = Book.AuthorId <= 0 ? existingBook.AuthorId : Book.AuthorId;
            existingBook.TotalPages = Book.TotalPages <= 0 ? existingBook.TotalPages : Book.TotalPages;
            existingBook.PublisherId = Book.PublisherId <= 0 ? existingBook.PublisherId : Book.PublisherId;
            existingBook.PublishedDate = Book.PublishedDate == existingBook.PublishedDate ? existingBook.PublishedDate : Book.PublishedDate;

            _libraryDB.Entry(existingBook).State = EntityState.Modified;

            // Save the changes to the database
            await _libraryDB.SaveChangesAsync();

            return existingBook;
        }
    }
}

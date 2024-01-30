using BookApi.DataDTO;
using BookApi.Models;

namespace BookApi.Services
{
    public interface IBookService
    {
        // Get all Books
        Task<IEnumerable<Book>> GetBooks();
        // Get Book By Id
        Task <Book?> GetBookById(int id);
        // Add New Book Info
        Task <Book?> AddBook(BookDTO Book);
        // Update Book Info
        Task<Book?> UpdateBook(BookDTO Book, int id);
        // Delete Book Info from DB
        Task<bool> DeleteBook(int id);

    }
}

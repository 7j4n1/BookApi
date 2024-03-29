using BookApi.DataDTO;
using BookApi.Models;

namespace BookApi.Services
{
    public interface IAuthorService
    {
        // Get all Authors
        Task<IEnumerable<Author>> GetAuthors();
        // Get Author By Id
        Task <Author?> GetAuthorById(int id);
        // Add New Author Info
        Task <Author?> AddAuthor(AuthorDTO author);
        // Update Author Info
        Task<Author?> UpdateAuthor(AuthorDTO author, int id);
        // Delete Author Info from DB
        Task<bool> DeleteAuthor(int id);



    }
}
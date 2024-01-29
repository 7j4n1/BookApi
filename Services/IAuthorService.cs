using BookApi.Models;

namespace BookApi;

public interface IAuthorService
{
    // Get all Authors
    public IEnumerable<Author> GetAuthors();
    // Get Author By Id
    public Author GetAuthorById(int id);
    // Add New Author Info
    public Author AddAuthor(Author author);
    // Update Author Info
    public Author UpdateAuthor(int id);
    // Delete Author Info from DB
    public bool DeleteAuthor(int id);



}
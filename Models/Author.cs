using BookApi.DataDTO;
using Newtonsoft.Json;

namespace BookApi.Models;

/// <summary>
/// Represents an author.
/// </summary>
public class Author: AuthorDTO
{
    public int Id {get; set;}
    
    public ICollection<Book>? Books { get; set; }

}

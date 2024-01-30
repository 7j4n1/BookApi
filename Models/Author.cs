using BookApi.DataDTO;

namespace BookApi.Models;

public class Author: AuthorDTO
{
    public int Id {get; set;}
    public ICollection<Book>? Books { get; set; }

}

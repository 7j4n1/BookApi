using BookApi.DataDTO;

namespace BookApi.Models;

public class Publisher: PublisherDTO
{
    public int Id {get; set;}
    public ICollection<Book>? Books { get; set; }
}

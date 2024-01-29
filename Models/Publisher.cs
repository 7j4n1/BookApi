namespace BookApi.Models;

public class Publisher: BaseAttr
{
    public required string Name { get; set; }

    public required ICollection<Book> Books { get; set; }
}

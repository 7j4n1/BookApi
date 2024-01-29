namespace BookApi.Models;

public class Author: BaseAttr
{
    public required string FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Bio {get; set;}

    public ICollection<Book>? Books { get; set; }

}

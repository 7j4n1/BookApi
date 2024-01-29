using System.ComponentModel.DataAnnotations;

namespace BookApi.Models;

public class Book: BaseAttr
{
    public required string Title { get; set; }

    [Range(1, int.MaxValue)]
    public required int TotalPages { get; set; }

    [RegularExpression(@"^\d{3}-\d{10}$")]
    public required string ISBN { get; set; }

    public required DateOnly PublishedDate { get; set; }

    // One-One Relationship (a book to one publisher)
    public required Publisher Publisher { get; set; }

    public required ICollection<Author> Authors { get; set; }
    
}

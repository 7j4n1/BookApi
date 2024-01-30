using System.ComponentModel.DataAnnotations;
using BookApi.Models;
using Microsoft.VisualBasic;

namespace BookApi.DataDTO;

public class BookDTO
{
    public required string Title { get; set; }

    [Range(1, int.MaxValue)]
    public required int TotalPages { get; set; }

    [RegularExpression(@"^\d{3}-\d{10}$")]
    public string? ISBN { get; set; }

    public string? PublishedDate { get; set; }

    public required int AuthorId { get; set; }

    public required int PublisherId { get; set; }
}

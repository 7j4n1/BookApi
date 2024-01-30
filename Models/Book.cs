using BookApi.DataDTO;

namespace BookApi.Models
{
    public class Book: BookDTO
    {
        public int Id {get; set;}
        // One-One Relationship (a book to one publisher)
        public Publisher? Publisher { get; set; }
        // One-One Relationship (a book to one author)
        public Author? Author { get; set; }
    
    }
}

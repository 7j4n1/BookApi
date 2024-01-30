using BookApi.DataDTO;
using Newtonsoft.Json;

namespace BookApi.Models
{
    public class Book: BookDTO
    {
        public int Id {get; set;}
        // One-One Relationship (a book to one publisher)
        [JsonIgnore]
        public Publisher? Publisher { get; set; }
        // One-One Relationship (a book to one author)
        [JsonIgnore]
        public Author? Author { get; set; }
    
    }
}

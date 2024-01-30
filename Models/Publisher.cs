using BookApi.DataDTO;
using Newtonsoft.Json;

namespace BookApi.Models;

public class Publisher: PublisherDTO
{
    public int Id {get; set;}
    [JsonIgnore]
    public ICollection<Book>? Books { get; set; }
}

using BookApi.Models;

namespace BookApi
{
    public class SubscriberMapper(string _message, Book _book)
    {
        public string message = _message;

        public Book book = _book;
        
    }
}
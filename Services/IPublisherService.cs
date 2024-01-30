using BookApi.DataDTO;
using BookApi.Models;

namespace BookApi.Services;

public interface IPublisherService
{

    // Get all Publishers
    Task<IEnumerable<Publisher>> GetPublishers();
    // Get Publisher By Id
    Task <Publisher?> GetPublisherById(int id);
    // Add New Publisher Info
    Task <Publisher?> AddPublisher(PublisherDTO Publisher);
    // Update Publisher Info
    Task<Publisher?> UpdatePublisher(PublisherDTO Publisher, int id);
    // Delete Publisher Info from DB
    Task<bool> DeletePublisher(int id);

}

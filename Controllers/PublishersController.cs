using BookApi;
using BookApi.Cache;
using BookApi.DataDTO;
using BookApi.Models;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PublishersController: ControllerBase
    {
        private readonly IPublisherService _publisherService;
        private readonly ICacheService _cacheService;
        public PublishersController(IPublisherService publisherService, ICacheService cacheService)
        {
            _publisherService = publisherService;
            _cacheService = cacheService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publisher>>> GetPublishers()
        {
            var getCacheData = _cacheService.GetData<IEnumerable<Publisher>>("publisher");

            if (getCacheData is not null)
            {
                return Ok(getCacheData);
            }

            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);

            getCacheData = await _publisherService.GetPublishers();
            // set the data into cache
            if (getCacheData is not null)
            {
                _ = _cacheService.SetData<IEnumerable<Publisher>>("publisher", getCacheData, expirationTime);
            }
            
            return Ok(getCacheData);
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Publisher>> GetPublisherById(int id)
        {
            var getCacheData = _cacheService.GetData<IEnumerable<Publisher>>("publisher");

            if (getCacheData is not null)
            {
                var filteredPublisher = getCacheData.Where(x => x.Id == id).FirstOrDefault();

                return Ok(filteredPublisher);
            }

            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            var getData = await _publisherService.GetPublisherById(id);
            Console.WriteLine(getData);
            // set the data into cache
            if (getData != null)
            {
                _ = _cacheService.SetData<Publisher>("publisher", getData, expirationTime);
                return Ok(getData);
            }
            
            // return NotFound("No Publisher with the id found.");
            return new ObjectResult(new ApiError () {Message = "No Publisher with the id found."})
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        [HttpPost]
        public async Task<ActionResult<Publisher>> PostPublisher(PublisherDTO Publisher)
        {
            if (string.IsNullOrEmpty(Publisher.Name))
            {
                return new ObjectResult(new ApiError () {Message = "Name is required"})
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            // Handle unexpected Exceptions
            try
            {
                // save data into database
                var result = await _publisherService.AddPublisher(Publisher);

                if (result is not null)
                {
                    return CreatedAtAction(nameof(GetPublisherById), new { id = result.Id}, result);
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                
                return BadRequest("Failed to add Publisher.");
            }

            return new ObjectResult(new ApiError () {Message = $"Publisher with the name {Publisher.Name} already exists"})
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutPublisher(int id, PublisherDTO Publisher)
        {

            // Handle unexpected Exceptions
            try
            {
                // update data in database
                var result = await _publisherService.UpdatePublisher(Publisher, id);

                if (result is not null)
                {
                    // remove Publisher from cache
                    _cacheService.RemoveData("publisher");
                    
                    return new ObjectResult(new ApiError () {Message = $"Publisher with the id ({id}) updated successful"})
                    {
                        StatusCode = StatusCodes.Status204NoContent
                    };
                }

                
            }
            catch (System.Exception)
            {
                return BadRequest("Failed to update Publisher info.");
            }

            return BadRequest($"Publisher id {id} specified is not found.");
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePublisher(int id)
        {

            // Handle unexpected Exceptions
            try
            {
                // update data in database
                var result = await _publisherService.DeletePublisher(id);

                if (result)
                {
                    // remove Publisher from cache
                    _cacheService.RemoveData("publisher");

                    return new ObjectResult(new ApiError () {Message = $"Publisher with the id ({id}) deleted successfully"})
                    {
                        StatusCode = StatusCodes.Status200OK
                    };
                }

                
            }
            catch (System.Exception)
            {
                
                return BadRequest("Failed to delete Publisher info.");
            }

            return NotFound($"Publisher id {id} specified is not found.");
            
        }

    }

    
    
}

using BookApi;
using BookApi.Cache;
using BookApi.DataDTO;
using BookApi.Models;
using BookApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly ICacheService _cacheService;
        public AuthorsController(IAuthorService authorService, ICacheService cacheService)
        {
            _authorService = authorService;
            _cacheService = cacheService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            var getCacheData = _cacheService.GetData<IEnumerable<Author>>("author");

            if (getCacheData is not null)
            {
                return Ok(getCacheData);
            }

            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);

            getCacheData = await _authorService.GetAuthors();
            // set the data into cache
            if (getCacheData is not null)
            {
                _ = _cacheService.SetData<IEnumerable<Author>>("author", getCacheData, expirationTime);
            }
            
            return Ok(getCacheData);
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthorById(int id)
        {
            var getCacheData = _cacheService.GetData<IEnumerable<Author>>("author");

            if (getCacheData is not null)
            {
                var filteredAuthor = getCacheData.Where(x => x.Id == id).FirstOrDefault();

                return Ok(filteredAuthor);
            }

            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            var getData = await _authorService.GetAuthorById(id);
            // set the data into cache
            if (getData != null)
            {
                _ = _cacheService.SetData<Author>("author", getData, expirationTime);
                return Ok(getData);
            }
            
            // return NotFound("No Author with the id found.");
            return new ObjectResult(new ApiError () {Message = "No Author with the id found."})
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
        }

        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(AuthorDTO author)
        {
            // E
            if (string.IsNullOrEmpty(author.FirstName))
            {
                return new ObjectResult(new ApiError () {Message = "First name is required"})
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            // Handle unexpected Exceptions
            try
            {
                // save data into database
                var result = await _authorService.AddAuthor(author);

                if (result is not null)
                {
                    return CreatedAtAction(nameof(GetAuthorById), new { id = result.Id}, result);
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                
                return BadRequest("Failed to add author.");
            }

            return new ObjectResult(new ApiError () {Message = $"Author with the name {author.FirstName} {author.LastName} already exists"})
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAuthor(int id, AuthorDTO author)
        {

            // Handle unexpected Exceptions
            try
            {
                // update data in database
                var result = await _authorService.UpdateAuthor(author, id);

                if (result is not null)
                {
                    // remove author from cache
                    _cacheService.RemoveData("author");
                    
                    return new ObjectResult(new ApiError () {Message = $"Author with the id ({id}) updated successful"})
                    {
                        StatusCode = StatusCodes.Status204NoContent
                    };
                }

                
            }
            catch (System.Exception)
            {
                return BadRequest("Failed to update author info.");
            }

            return BadRequest($"Author id {id} specified is not found.");
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuthor(int id)
        {

            // Handle unexpected Exceptions
            try
            {
                // update data in database
                var result = await _authorService.DeleteAuthor(id);

                if (result)
                {
                    // remove author from cache
                    _cacheService.RemoveData("author");

                    return new ObjectResult(new ApiError () {Message = $"Author with the id ({id}) deleted successfully"})
                    {
                        StatusCode = StatusCodes.Status200OK
                    };
                }

                
            }
            catch (System.Exception)
            {
                
                return BadRequest("Failed to delete author info.");
            }

            return NotFound($"Author id {id} specified is not found.");
            
        }

    }

    
    
}

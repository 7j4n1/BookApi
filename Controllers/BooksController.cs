using BookApi.Cache;
using BookApi.DataDTO;
using BookApi.Models;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi
{
    public class BooksController: ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IPublisherHelper _publisherHelper;
        private readonly IAuthorHelper _authorHelper;
        private readonly ICacheService _cacheService;
        public BooksController(IBookService bookService, IPublisherHelper publisherHelper, ICacheService cacheService, IAuthorHelper authorHelper)
        {
            _publisherHelper = publisherHelper;
            _cacheService = cacheService;
            _authorHelper = authorHelper;
            _bookService = bookService;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var getCacheData = _cacheService.GetData<IEnumerable<Book>>("book");

            if (getCacheData is not null)
            {
                return Ok(getCacheData);
            }

            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);

            getCacheData = await _bookService.GetBooks();
            // set the data into cache
            if (getCacheData is not null)
            {
                _ = _cacheService.SetData<IEnumerable<Book>>("book", getCacheData, expirationTime);
            }
            
            return Ok(getCacheData);
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Publisher>> GetBookById(int id)
        {
            var getCacheData = _cacheService.GetData<IEnumerable<Book>>("book");

            if (getCacheData is not null)
            {
                var filteredPublisher = getCacheData.Where(x => x.Id == id).FirstOrDefault();

                return Ok(filteredPublisher);
            }

            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            var getData = await _bookService.GetBookById(id);
            // set the data into cache
            if (getData != null)
            {
                _ = _cacheService.SetData<Book>("book", getData, expirationTime);
                return Ok(getData);
            }
            
            // return NotFound("No Book with the id found.");
            return new ObjectResult(new ApiError () {Message = "No Publisher with the id found."})
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        /// <summary>
        /// Creates a new book and adds it to the database.
        /// </summary>
        /// <param name="book">The book data to be added.</param>
        /// <returns>An ActionResult containing the created book if successful, or an error message if unsuccessful.</returns>
        [HttpPost]
        public async Task<ActionResult<Book>> PostPublisher(BookDTO book)
        {
            if (string.IsNullOrEmpty(book.Title))
            {
                return new ObjectResult(new ApiError () {Message = "Title is required"})
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            if (book.TotalPages <= 0 )
            {
                return new ObjectResult(new ApiError () {Message = "Total Page is required"})
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            if ((book.PublisherId <= 0 ) || (book.AuthorId <= 0 ))
            {
                return new ObjectResult(new ApiError () {Message = "Valid Publisher/Author Id is required"})
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            if (await _authorHelper.IsAuthorExistById(book.AuthorId)  )
            {
                return new ObjectResult(new ApiError () {Message = $"Author Id ({book.AuthorId}) does not exist"})
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            if (await _publisherHelper.IsPublisherExistById(book.PublisherId)  )
            {
                return new ObjectResult(new ApiError () {Message = $"Publisher Id ({book.PublisherId}) does not exist"})
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }



            // Handle unexpected Exceptions
            try
            {
                // save data into database
                var result = await _bookService.AddBook(book);

                if (result is not null)
                {
                    return CreatedAtAction(nameof(GetBookById), new { id = result.Id}, result);
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                
                return BadRequest("Failed to add Book.");
            }

            return new ObjectResult(new ApiError () {Message = $"Book with the title {book.Title} already exists"})
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutBook(int id, BookDTO book)
        {

            // Handle unexpected Exceptions
            try
            {
                // update data in database
                var result = await _bookService.UpdateBook(book, id);

                if (result is not null)
                {
                    // remove Book from cache
                    _cacheService.RemoveData("book");
                    
                    return new ObjectResult(new ApiError () {Message = $"Book with the id ({id}) updated successful"})
                    {
                        StatusCode = StatusCodes.Status204NoContent
                    };
                }

                
            }
            catch (System.Exception)
            {
                return BadRequest("Failed to update Book info.");
            }

            return BadRequest($"Book id {id} specified is not found.");
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {

            // Handle unexpected Exceptions
            try
            {
                // update data in database
                var result = await _bookService.DeleteBook(id);

                if (result)
                {
                    // remove Book from cache
                    _cacheService.RemoveData("book");

                    return new ObjectResult(new ApiError () {Message = $"Book with the id ({id}) deleted successfully"})
                    {
                        StatusCode = StatusCodes.Status200OK
                    };
                }

                
            }
            catch (System.Exception)
            {
                
                return BadRequest("Failed to delete Book info.");
            }

            return NotFound($"Book id {id} specified is not found.");
            
        }

    }

    
    
}

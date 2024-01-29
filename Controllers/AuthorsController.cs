using BookApi.Cache;
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
            if (getData is not null)
            {
                _ = _cacheService.SetData<Author>("author", getData, expirationTime);
            }
            return Ok(getData);
        }

        // public async Task<ActionResult<>>
    }
}

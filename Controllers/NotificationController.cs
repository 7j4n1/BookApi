using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NotificationController(IMessageStorageService messageStorageService): ControllerBase
    {
        private readonly IMessageStorageService _messageStorage = messageStorageService;

        [HttpGet]
        public async Task<ActionResult> GetNotification()
        {
            var message = await _messageStorage.GetMessageAsync();
            if (string.IsNullOrEmpty(message))
            {
                return NotFound("No new book published");
            }
            return Ok(message);
        }
    }
}

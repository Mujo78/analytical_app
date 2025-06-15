using Microsoft.AspNetCore.Mvc;
using server.Services.IServices;

namespace server.Controllers
{
    [Route("api/db")]
    [ApiController]
    public class DBController(IDBService dbService) : ControllerBase
    {
        private readonly IDBService _dbService = dbService;

        [HttpPost("create")]
        public async Task<IActionResult> CreateIndexes()
        {
            await _dbService.CreateIndexes();
            return Ok("Indexes created successfully.");
        }

        [HttpPost("drop")]
        public async Task<IActionResult> DropIndexes()
        {
            await _dbService.DropIndexes();
            return Ok("Indexes dropped successfully.");
        }
    }
}

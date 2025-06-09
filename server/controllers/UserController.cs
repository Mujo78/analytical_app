using Microsoft.AspNetCore.Mvc;
using server.Services.IServices;

namespace server.Controllers
{
    [Route("api/users/")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService userService = userService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetUsers()
        {
            var users = await userService.GetTopReputationUsersAsync();
            return Ok(users);
        }
    }
}

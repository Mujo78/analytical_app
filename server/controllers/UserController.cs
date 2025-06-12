using Microsoft.AspNetCore.Mvc;
using server.DTO.User;
using server.Services.IServices;

namespace server.Controllers
{
    [Route("api/users/")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService userService = userService;

        /// <summary>
        /// Gets the top reputation users.
        /// Entity Framework Core is used to query the database.
        /// Simple SELECT query to retrieve the top users based on their reputation.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetUsers([FromQuery] bool useDapper)
        {
            var users = await userService.GetTopReputationUsersAsync(useDapper);
            return Ok(users);
        }

        /// <summary>
        /// Gets the user analytics by user ID.
        /// Entity Framework Core is used to query the database.
        /// Complex SELECT query to retrieve user analytics including posts, comments, and scores.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetUserAnalytics(int id, [FromQuery] bool useDapper)
        {
            try
            {
                var userAnalytics = await userService.GetUsersAnalyticsAsync(id, useDapper);
                return Ok(userAnalytics);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"User with ID {id} not found.");
            }
        }

        /// <summary>
        /// Creates a new user.
        /// Entity Framework Core is used to insert the user into the database.
        /// Simple INSERT query to add a new user.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserDTO user, [FromQuery] bool useDapper)
        {
            if (user == null || !ModelState.IsValid)
            {
                return BadRequest("User data is invalid.");
            }

            try
            {
                await userService.CreateUserAsync(user, useDapper);
                return Ok("User created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while creating the user: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing user.
        /// Entity Framework Core is used to update the user in the database.
        /// Simple UPDATE query to modify user details.
        /// </summary>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDTO userUpdateDTO, int userId, [FromQuery] bool useDapper)
        {
            if (userUpdateDTO == null || !ModelState.IsValid)
            {
                return BadRequest("User update data is invalid.");
            }

            try
            {
                var updatedUser = await userService.UpdateUserAsync(userUpdateDTO, userId, useDapper);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while updating the user: {ex.Message}");
            }
        }

        /// <summary>
        /// This endpoint will add bonus reputation points to specific users.
        /// Users who has at least 5 posts and 1 comment will receive 10 bonus reputation points.
        /// Entity Framework Core is used to update the user in the database.
        /// Complex UPDATE query to modify user reputation points.
        /// </summary>
        [HttpPut("distribute-bonus")]
        public async Task<IActionResult> DistributeReputationBonus([FromQuery] bool useDapper)
        {
            if (useDapper)
            {
                await userService.DistributeBonusReputationWithDapperAsync();
            }
            else
            {
                await userService.DistributeBonusReputationAsync();
            }

            return Ok("Bonus reputation points distributed successfully using Dapper.");
        }

    }
}

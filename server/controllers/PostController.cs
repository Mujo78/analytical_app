using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.DTO.Post;
using server.Services.IServices;

namespace server.Controllers
{
    [Route("api/posts/")]
    [ApiController]
    public class PostController(IPostService postService) : ControllerBase
    {
        private readonly IPostService postService = postService;

        /// <summary>
        /// Creates a new post.
        /// Entity Framework Core is used to insert the post into the database.
        /// Complex INSERT query to add a new post.
        /// </summary>
        [HttpPost("/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreatePost([FromBody] CreateDTO post, int userId)
        {
            if (post == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await postService.CreatePostAsync(post, userId);
                return Ok("Post created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating post: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a post by postId.
        /// This will use Entity Framework to delete the post.
        /// </summary>
        [HttpDelete("{postId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePost(int postId, int userId)
        {
            if (postId <= 0)
            {
                return BadRequest("Invalid post ID.");
            }

            try
            {
                var deletedPostId = await postService.DeletePostAsync(postId, userId);
                return Ok(deletedPostId);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting post: {ex.Message}");
            }
        }
    }
}

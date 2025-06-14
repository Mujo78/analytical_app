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
        /// Complex INSERT query to add a new post.
        /// </summary>
        [HttpPost("/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreatePost([FromBody] CreateDTO post, int userId, [FromQuery] bool useDapper = false)
        {
            if (post == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await postService.CreatePostAsync(post, userId, useDapper);
                return Ok("Post created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating post: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a post by postId.
        /// </summary>
        [HttpDelete("{postId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePost(int postId, int userId, [FromQuery] bool useDapper = false)
        {
            if (postId <= 0)
            {
                return BadRequest("Invalid post ID.");
            }

            try
            {
                int deletedPostId;
                if (useDapper)
                {
                    deletedPostId = await postService.DeletePostWithDapperAsync(postId, userId);
                }
                else
                {
                    deletedPostId = await postService.DeletePostAsync(postId, userId);
                }
                return Ok(deletedPostId);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting post: {ex.Message}");
            }
        }
    }
}

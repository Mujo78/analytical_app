using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Services.IServices;

namespace server.Controllers
{
    [Route("api/comments/")]
    [ApiController]
    public class CommentController(ICommentService commentService) : ControllerBase
    {
        private readonly ICommentService _commentService = commentService;

        /// <summary>
        /// Retrieves comments by postID - comments for posts - latest 10.
        /// /// </summary>
        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentsByPostIdAsync(int postId)
        {
            var comments = await _commentService.GetCommentsByPostIdAsync(postId);
            if (comments == null || comments.Count == 0)
            {
                return NotFound("No comments found for this post.");
            }
            return Ok(comments);
        }

        /// <summary>
        /// Deletes a comment by postId and commentId.
        /// This will use Entity Framework to delete the comment.
        /// </summary>
        [HttpDelete("{postId}/delete/{commentId}")]
        public async Task<IActionResult> DeleteComment(int postId, int commentId)
        {
            var commentToDeleteId = await _commentService.DeleteCommentAsync(commentId, postId);
            return Ok(commentToDeleteId);
        }
    }
}

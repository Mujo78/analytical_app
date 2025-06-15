using System;

namespace server.DTO.Comment;

public class CommentDTO
{
    public int CommentId { get; set; }
    public int PostId { get; set; }
    public int? UserId { get; set; }
    public string Text { get; set; }
    public DateTime CreationDate { get; set; }
    public int Score { get; set; }
    public string UserDisplayName { get; set; }
}

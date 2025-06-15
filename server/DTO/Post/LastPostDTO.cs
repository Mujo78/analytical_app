using System;
using server.DTO.Comment;

namespace server.DTO.Post;

public class LastPostDTO
{
    public int Id { get; set; }
    public string Body { get; set; }
    public DateTime? ClosedDate { get; set; }
    public int? CommentCount { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? LastEditDate { get; set; }
    public int? OwnerUserId { get; set; }
    public int PostTypeId { get; set; }
    public int Score { get; set; }
    public string? Tags { get; set; }
    public string? Title { get; set; }
    public int ViewCount { get; set; }
    public List<CommentDTO> Comments { get; set; }
}

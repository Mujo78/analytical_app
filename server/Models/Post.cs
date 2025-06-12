using System;
using System.ComponentModel.DataAnnotations;

namespace server.Models;

public class Post
{
    [Key]
    public int Id { get; set; }
    public int? AcceptedAnswerId { get; set; }
    public int? AnswerCount { get; set; }
    [Required(ErrorMessage = "Body is required.")]
    public string Body { get; set; }
    public DateTime? ClosedDate { get; set; }
    public int? CommentCount { get; set; }
    public DateTime? CommunityOwnedDate { get; set; }
    public DateTime CreationDate { get; set; }
    public int? FavoriteCount { get; set; }
    public DateTime LastActivityDate { get; set; }
    public DateTime? LastEditDate { get; set; }
    [MaxLength(40, ErrorMessage = "Last editor display name cannot exceed 40 characters.")]
    public string? LastEditorDisplayName { get; set; }
    public int? LastEditorUserId { get; set; }
    public int? OwnerUserId { get; set; }
    public int? ParentId { get; set; }
    public int PostTypeId { get; set; }
    public int Score { get; set; }
    [MaxLength(150, ErrorMessage = "Tags cannot exceed 150 characters.")]
    public string? Tags { get; set; }
    [MaxLength(250, ErrorMessage = "Title cannot exceed 250 characters.")]
    public string? Title { get; set; }
    public int ViewCount { get; set; }

    public User OwnerUser { get; set; }
    public ICollection<Comment> Comments { get; set; }
}

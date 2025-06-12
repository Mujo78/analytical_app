using System;
using System.ComponentModel.DataAnnotations;

namespace server.DTO.Post;

public class CreateDTO
{
    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(250, ErrorMessage = "Title cannot exceed 250 characters.")]
    [MinLength(5, ErrorMessage = "Title must be at least 5 characters long.")]
    public required string Title { get; set; }
    [Required(ErrorMessage = "Body is required.")]
    public required string Body { get; set; }
    public int PostTypeId { get; set; } = 1;
    public string Tags { get; set; } = string.Empty;
}

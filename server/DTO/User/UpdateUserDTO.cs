using System.ComponentModel.DataAnnotations;

namespace server.DTO.User;

public class UpdateUserDTO
{
    [Required(ErrorMessage = "Display name is required.")]
    [MaxLength(40, ErrorMessage = "Display name cannot exceed 40 characters.")]
    public required string DisplayName { get; set; }
    [MaxLength(40, ErrorMessage = "Email name cannot exceed 40 characters.")]
    public string Email { get; set; } = string.Empty;
    [MaxLength(100, ErrorMessage = "Location name cannot exceed 100 characters.")]
    public string Location { get; set; } = string.Empty;
    public string AboutMe { get; set; } = string.Empty;
    public int Age { get; set; }
    [MaxLength(200, ErrorMessage = "WebsiteURL name cannot exceed 200 characters.")]
    public string WebsiteUrl { get; set; } = string.Empty;
}

using System;
using System.ComponentModel.DataAnnotations;

namespace server.Models;

public class Comment
{
    [Key]
    public int Id { get; set; }
    public int PostId { get; set; }
    public int? Score { get; set; }
    public string Text { get; set; }
    public DateTime CreationDate { get; set; }
    public int? UserId { get; set; }

    public Post Post { get; set; }
    public User User { get; set; }
}

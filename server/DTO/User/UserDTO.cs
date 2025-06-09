using System;

namespace server.DTO.User;

public class UserDTO
{
    public int Id { get; set; }
    public string AboutMe { get; set; }
    public int Age { get; set; }
    public DateTime CreationDate { get; set; }
    public string DisplayName { get; set; }
    public int DownVotes { get; set; }
    public string EmailHash { get; set; }
    public DateTime LastAccessDate { get; set; }
    public string Location { get; set; }
    public int Reputation { get; set; }
    public int UpVotes { get; set; }
    public int Views { get; set; }
}

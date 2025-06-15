using System;

namespace server.DTO.User;

public class TopUserReputationDTO
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
    public string DisplayName { get; set; }
    public int DownVotes { get; set; }
    public int Reputation { get; set; }
    public int UpVotes { get; set; }
    public int Views { get; set; }
}

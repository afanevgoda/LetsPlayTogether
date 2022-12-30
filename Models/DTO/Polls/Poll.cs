namespace LetsPlayTogether.Models.DTO;

public class Poll{
    public string Id { get; set; }
    public List<string> PlayerIds { get; set; }
    public List<string> GameIds { get; set; }
    public List<PollAppVotes> Votes { get; set; }
    public List<ResultVotes> Results { get; set; }
}

public class ResultVotes{
    public string AppId { get; set; }

    public List<int> Rating { get; set; }
}
namespace LetsPlayTogether.Models.DTO;

public class SubmitPollRequest{
    public string Id { get; set; }

    public List<PollAppVotes> Votes { get; set; }
}

public class PollAppVotes{
    public string AppId { get; set; }

    public int Rating { get; set; }
}
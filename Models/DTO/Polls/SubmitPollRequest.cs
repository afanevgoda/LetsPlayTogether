namespace LetsPlayTogether.Models.DTO;

public class SubmitPollRequestDto{
    public string Id { get; set; }
    
    public List<GameDto> Games { get; set; }

    public List<PollAppVotesDto> Votes { get; set; }
}

public class PollAppVotesDto{
    public string AppId { get; set; }

    public int Rating { get; set; }
}
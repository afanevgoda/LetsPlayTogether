namespace LetsPlayTogether.Models.DTO.Polls;

public class SubmitPollRequestDto{ 
    public string Id { get; set; } = null!;
    public List<PollAppVotesDto> Votes { get; set; } = null!;
}

public class PollAppVotesDto{
    public string AppId { get; set; } = null!;
    public int Rating { get; set; }
}
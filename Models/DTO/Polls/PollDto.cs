namespace LetsPlayTogether.Models.DTO.Polls;

public class PollDto{
    public string Id { get; set; }  = null!;
    public List<string> PlayerIds { get; set; }  = null!;
    public List<PollMatchedGameDto> Games { get; set; }  = null!;
    public List<PollAppVotesDto> Votes { get; set; }  = null!;
    public List<ResultVotesDto> Results { get; set; }  = null!;
}

public class ResultVotesDto{
    public string AppId { get; set; }  = null!;
    public List<int> Rating { get; set; }  = null!;
}

public class PollMatchedGameDto{
    public string AppId { get; set; }  = null!;
    public string Name { get; set; }  = null!;
    public string HeaderImage { get; set; }  = null!;
    public int NumberOfOwningPlayers { get; set; }
    public List<string>? PlayersThatDontHaveGame { get; set; }
}
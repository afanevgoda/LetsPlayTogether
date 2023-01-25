namespace LetsPlayTogether.Models.DTO;

public class PollDto{
    public string Id { get; set; }
    public List<string> PlayerIds { get; set; }
    public List<PollMatchedGameDto> Games { get; set; }
    public List<PollAppVotesDto> Votes { get; set; }
    public List<ResultVotesDto> Results { get; set; }
}

public class ResultVotesDto{
    public string AppId { get; set; }

    public List<int> Rating { get; set; }
}

public class PollMatchedGameDto{
    public string AppId { get; set; }

    public string Name { get; set; }

    public string HeaderImage { get; set; }

    public int NumberOfOwningPlayers { get; set; }

    public List<string>? PlayersThatDontHaveGame { get; set; }
}
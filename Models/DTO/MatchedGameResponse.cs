namespace LetsPlayTogether.Models.DTO;

public class MatchedGameResponse{
    public List<GameDto> MatchedGames { get; set; }
    public string PollId { get; set; }
}
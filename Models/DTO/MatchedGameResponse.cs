namespace LetsPlayTogether.Models.DTO;

public class MatchedGameResponse{
    public List<GameDto> MatchedGames { get; set; } = null!;
    public string PollId { get; set; } = null!;
}
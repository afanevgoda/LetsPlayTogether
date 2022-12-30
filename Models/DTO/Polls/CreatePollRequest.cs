namespace LetsPlayTogether.Models.DTO;

public class CreatePollRequest{
    public List<string> PlayersIds { get; set; }
    public List<string> GamesIds { get; set; }
}
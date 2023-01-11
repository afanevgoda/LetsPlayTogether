namespace LetsPlayTogether.Models.DTO;

public class CreatePollRequestDto{
    public List<string> PlayersIds { get; set; }
    public List<string> GamesIds { get; set; }
}
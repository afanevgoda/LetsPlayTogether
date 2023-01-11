namespace LetsPlayTogether.Models.Steam.Responses;

public class GetPlayerSummariesDto{
    public PlayersResponse Response { get; set; }
}

public class PlayersResponse{
    public List<SteamPlayerDto> Players { get; set; }
}
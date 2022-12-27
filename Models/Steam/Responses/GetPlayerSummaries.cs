namespace LetsPlayTogether.Models.Steam.Responses;

public class GetPlayerSummaries{
    public PlayersResponse Response { get; set; }
}

public class PlayersResponse{
    public List<SteamPlayer> Players { get; set; }
}
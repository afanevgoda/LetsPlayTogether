namespace LetsPlayTogether.Models.Steam.Responses;

public class GetPlayerSummariesDto{
    public PlayersResponse Response { get; set; } = null!;
}

public class PlayersResponse{
    public List<SteamPlayerDto> Players { get; set; } = null!;
}
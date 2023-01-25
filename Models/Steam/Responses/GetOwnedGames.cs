using Newtonsoft.Json;

namespace LetsPlayTogether.Models.Steam.Responses;

public class GetOwnedGamesDto{
    public GamesResponse Response { get; set; } = null!;
}

public class GamesResponse{
    [JsonProperty("games")]    
    public List<OwnedGame> Games { get; set; } = null!;

    [JsonProperty("game_count")]
    public int Count { get; set; }
}

public class OwnedGame{
    public string AppId { get; set; } = null!;
}
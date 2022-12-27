using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace LetsPlayTogether.Models.Steam;

public class GetOwnedGames{
    public GamesResponse Response { get; set; }
}

public class GamesResponse{
    [JsonProperty("games")]    
    public List<SteamGame> Games { get; set; }
    [JsonProperty("game_count")]
    public int Count { get; set; }
}
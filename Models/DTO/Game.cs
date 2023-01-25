using Newtonsoft.Json;

namespace LetsPlayTogether.Models.DTO;

public class GameDto{
    [JsonProperty("steam_appid")]
    public string AppId { get; set; } = null!;

    public string Name { get; set; } = null!;
    
    [JsonProperty("header_image")]
    public string HeaderImage { get; set; } = null!;
    
    public bool IsOk { get; set; }
    
    public string Tags { get; set; } = null!;
    
    public int NumberOfOwningPlayers { get; set; }
    
    public List<string>? PlayersThatDontHaveGame { get; set; }
}
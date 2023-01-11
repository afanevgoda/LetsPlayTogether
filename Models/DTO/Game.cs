using Newtonsoft.Json;

namespace LetsPlayTogether.Models.DTO;

public class GameDto{
    [JsonProperty("steam_appid")]
    public string AppId { get; set; }

    public string Name { get; set; }
    
    [JsonProperty("header_image")]
    public string HeaderImage { get; set; }
    
    public bool IsOk { get; set; }
    
    public string Tags { get; set; }
    
    public int NumberOfOwningPlayers { get; set; }
    
    public List<string>? PlayersThatDontHaveGame { get; set; }
}
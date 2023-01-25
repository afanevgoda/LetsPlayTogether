using Newtonsoft.Json;

namespace LetsPlayTogether.Models.Steam;

public class SteamGameDto{
    [JsonProperty("steam_appid")]
    public string AppId { get; set; } = null!;
    public float PlaytimeForever { get; set; }
    public string Name { get; set; } = null!;
    [JsonProperty("img_icon_url")]
    public string IconUrl { get; set; } = null!;
    [JsonProperty("header_image")]
    public string HeaderImage { get; set; } = null!;
    [JsonProperty("categories")]
    public List<CategoryDto> Categories { get; set; } = null!;
}

public class CategoryDto{
    public int Id { get; set; }
    
    public string Description { get; set; } = null!;
}
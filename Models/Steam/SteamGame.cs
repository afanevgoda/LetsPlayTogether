using Newtonsoft.Json;

namespace LetsPlayTogether.Models.Steam;

public class SteamGame{
    [JsonProperty("steam_appid")]
    public string AppId { get; set; }
    public float PlaytimeForever { get; set; }
    public string Name { get; set; }
    [JsonProperty("img_icon_url")]
    public string IconUrl { get; set; }
    [JsonProperty("header_image")]
    public string HeaderImage { get; set; }
    [JsonProperty("categories")]
    public List<Category> Categories { get; set; }
}

public class Category{
    public int Id { get; set; }
    
    public string Description { get; set; }
}
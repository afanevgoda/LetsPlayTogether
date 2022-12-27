using Newtonsoft.Json;

namespace LetsPlayTogether.Models.Steam;

public class SteamGame{
    public int AppId { get; set; }
    public float PlaytimeForever { get; set; }
    public string Name { get; set; }
    [JsonProperty("img_icon_url")]
    public string IconUrl { get; set; }
    [JsonProperty("header_image")]
    public string HeaderImage { get; set; }
}
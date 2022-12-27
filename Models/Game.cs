using Newtonsoft.Json;

namespace LetsPlayTogether.Models;

public class Game{
    
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string IconUrl { get; set; }
    
    [JsonProperty("header_image")]
    public string HeaderImage { get; set; }
}
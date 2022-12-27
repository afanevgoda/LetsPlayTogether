namespace LetsPlayTogether.Models;

public class Player{
    
    public string Id { get; set; }
    
    public string Nickname { get; set; }
    
    public string AvatarUrl { get; set; }
    
    public List<string> OwnedAppIds { get; set; }
    
    public List<Game> Games { get; set; }
}
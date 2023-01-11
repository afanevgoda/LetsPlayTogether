using LetsPlayTogether.Models.DTO;

namespace LetsPlayTogether.Models;

public class PlayerDto{
    
    public string Id { get; set; }
    
    public string Nickname { get; set; }
    
    public string AvatarUrl { get; set; }
    
    public List<string> OwnedAppIds { get; set; }
    
    public List<GameDto> Games { get; set; }
}
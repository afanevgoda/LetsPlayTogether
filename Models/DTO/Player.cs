namespace LetsPlayTogether.Models.DTO;

public class PlayerDto{
    public string Id { get; set; } = null!;
    public string Nickname { get; set; } = null!;
    public string AvatarUrl { get; set; } = null!;
    public List<string> OwnedAppIds { get; set; } = null!;
    public List<GameDto> Games { get; set; } = null!;
}
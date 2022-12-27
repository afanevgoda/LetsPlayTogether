namespace LetsPlayTogether.Models.Steam;

public class SteamPlayer{
    public string SteamId { get; set; }
    public int CommunityVisibilityState { get; set; }
    public int ProfileState { get; set; }
    public string PersonaName { get; set; }
    public int CommentPermission { get; set; }
    public string ProfileUrl { get; set; }
    public string Avatar { get; set; }
    public string AvatarMedium { get; set; }
    public string AvatarFull { get; set; }

    public string LastLogOff { get; set; }

    public string PersonaState { get; set; }
}
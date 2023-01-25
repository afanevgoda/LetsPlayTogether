namespace LetsPlayTogether.Models.Steam;

public class SteamPlayerDto{ 
    public string SteamId { get; set; } = null!;
    // public int CommunityVisibilityState { get; set; }
    // public int ProfileState { get; set; }
    public string PersonaName { get; set; } = null!;
    // public int CommentPermission { get; set; }
    // public string ProfileUrl { get; set; }
    public string Avatar { get; set; } = null!;
    // public string AvatarMedium { get; set; }
    // public string AvatarFull { get; set; }
    //
    // public string LastLogOff { get; set; }
    //
    // public string PersonaState { get; set; }
}
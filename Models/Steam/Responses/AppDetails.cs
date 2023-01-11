namespace LetsPlayTogether.Models.Steam.Responses;

public class AppDetailsDto{
    public bool Success { get; set; }
    
    public SteamGameDto Data { get; set; }
}
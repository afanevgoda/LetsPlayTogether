using LetsPlayTogether.Models;

namespace LetsPlayTogether.Services;

public interface ISteamService{

    Task<List<Player>> GetPlayersInfo(List<string> userIds);

    Task<List<Game>> GetMatchedGames(List<string> userIds);
}
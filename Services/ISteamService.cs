using LetsPlayTogether.Models;
using LetsPlayTogether.Models.DTO;

namespace LetsPlayTogether.Services;

public interface ISteamService{

    Task<List<Player>> GetPlayersInfo(List<string> userIds);

    Task<List<Game>> GetMatchedGames(List<string> userIds);

    Task<List<Game>> GetGames(List<string> gameAppIds);
}
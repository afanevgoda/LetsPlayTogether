using LetsPlayTogether.Models;
using LetsPlayTogether.Models.DTO;

namespace LetsPlayTogether.Services;

public interface ISteamService{

    Task<List<PlayerDto>> GetPlayersInfo(IEnumerable<string> userIds);

    Task<List<GameDto>> GetMatchedGames(IEnumerable<string> userIds);

    Task<List<GameDto>> GetGames(IEnumerable<string> gameAppIds);
}
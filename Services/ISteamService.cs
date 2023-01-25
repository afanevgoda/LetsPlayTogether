using LetsPlayTogether.Models.DTO;

namespace LetsPlayTogether.Services;

public interface ISteamService{
    Task<List<string>> GetPlayerIds(IEnumerable<string> playersUrls);

    Task<List<PlayerDto>> GetPlayersInfoByIds(IEnumerable<string> userIds);

    Task<List<GameDto>> GetMatchedGames(IEnumerable<string> userIds);

    Task<List<GameDto>> GetGames(IEnumerable<string> gameAppIds);
}
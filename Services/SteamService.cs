using AutoMapper;
using DataAccess.Models;
using DataAccess.Repositories;
using LetsPlayTogether.Models;
using LetsPlayTogether.Models.DTO;
using LetsPlayTogether.Models.Steam;
using LetsPlayTogether.Models.Steam.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LetsPlayTogether.Services;

public class SteamService : ISteamService{
    private readonly string _steamKey;
    private readonly IMapper _mapper;
    private readonly IGameRepository _games;
    private readonly IPollService _pollService;

    public SteamService(IConfiguration configuration, IMapper mapper, IGameRepository games, IPollService pollService) {
        _steamKey = configuration["SteamApiKey"];
        _mapper = mapper;
        _games = games;
        _pollService = pollService;
    }

    public async Task<List<GameDto>> GetMatchedGames(IEnumerable<string> userIds) {
        var playersInfo = await GetPlayersInfo(userIds);
        var appIdToPlayerWhoDontOwnIt = new Dictionary<string, List<string>>();
        var appIdToNumberOfOwningPlayers = new Dictionary<string, int>();

        var appIdsTotal = playersInfo.SelectMany(x => x.OwnedAppIds);
        foreach (var player in playersInfo) {
            foreach (var ownedAppId in player.OwnedAppIds) {
                var playersWhoDontOwnGame = playersInfo
                    .Where(x => !x.OwnedAppIds.Contains(ownedAppId))
                    .Select(x => x.Nickname).ToList();

                if (!appIdToPlayerWhoDontOwnIt.ContainsKey(ownedAppId))
                    appIdToPlayerWhoDontOwnIt.Add(ownedAppId, playersWhoDontOwnGame);

                var numberOfOwningPlayers = playersInfo
                    .Count(x => x.OwnedAppIds.Contains(ownedAppId));

                if (!appIdToNumberOfOwningPlayers.ContainsKey(ownedAppId))
                    appIdToNumberOfOwningPlayers.Add(ownedAppId, numberOfOwningPlayers);
            }
        }

        var gamesInfo = await GetGames(appIdsTotal.ToList());
        foreach (var gameInfo in gamesInfo) {
            gameInfo.PlayersThatDontHaveGame = appIdToPlayerWhoDontOwnIt[gameInfo.AppId];
            gameInfo.NumberOfOwningPlayers = appIdToNumberOfOwningPlayers[gameInfo.AppId];
        }

        await _pollService.CreatePoll(new List<string>(), _mapper.Map<List<PollMatchedGame>>(gamesInfo));

        return gamesInfo;
    }

    public async Task<List<GameDto>> GetGames(IEnumerable<string> gameAppIds) {
        var matchedGames = new List<GameDto>();

        var gameInfoList = await GetGameInfoFromApiIfRequired(gameAppIds);
        matchedGames = gameInfoList.Where(x => x != null &&
                                               x.IsOk &&
                                               x.Tags != null &&
                                               (x.Tags.Contains("Multi-player, , ") ||
                                                x.Tags.Contains("Co-op") ||
                                                x.Tags.Contains("Online Co-op")))
            .ToList();

        return matchedGames;
    }

    public async Task<List<PlayerDto>> GetPlayersInfo(IEnumerable<string> userIds) {
        using var httpClient = new HttpClient();

        var parameters = string.Join(",", userIds);

        var result = await httpClient.GetAsync(
            $"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={_steamKey}&SteamIds={parameters}");

        var playersResponseResult = await result.Content.ReadAsStringAsync();
        var steamPlayer = JObject.Parse(playersResponseResult)["response"]?["players"]?
            .Select(x => x.ToObject<SteamPlayerDto>()).ToList();
        var players = _mapper.Map<List<PlayerDto>>(steamPlayer);

        foreach (var player in players) {
            player.OwnedAppIds = await GetListOfOwnedAppIds(player.Id);
        }

        return players;
    }

    public async Task<GameDto> GetGameInfo(string appId) {
        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync($"https://store.steampowered.com/api/appdetails?appids={appId}");
        var gameString = await response.Content.ReadAsStringAsync();
        var desGame = JsonConvert.DeserializeObject<Dictionary<string, AppDetailsDto>>(gameString);
        var appDetails = desGame?[appId];

        if (appDetails?.Data == null)
            return new GameDto {
                AppId = appId,
                IsOk = false
            };

        var game = _mapper.Map<GameDto>(appDetails);
        return game;
    }

    private async Task<List<string>> GetListOfOwnedAppIds(string userId) {
        using var httpClient = new HttpClient();
        var gamesResponse = await httpClient.GetAsync(
            $"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={_steamKey}&steamid={userId}&format=json");
        var stringified = await gamesResponse.Content.ReadAsStringAsync();
        var desGames = JsonConvert.DeserializeObject<GetOwnedGamesDto>(stringified);
        return desGames?.Response.Games.Select(x => x.AppId).ToList() ?? new List<string>();
    }

    private async Task<List<GameDto>> GetGameInfoFromApiIfRequired(IEnumerable<string> appIds) {
        List<GameDto> result = new List<GameDto>();

        var gamesFromDb = await _games.GetByAppIdList(appIds);
        var gamesFromApi = appIds.Where(x => !gamesFromDb.Select(x => x.AppId).Contains(x));

        foreach (var appId in gamesFromApi) {
            try {
                var gameFromApi = await GetGameInfo(appId);
                var gameData = _mapper.Map<DataAccess.Models.Game>(gameFromApi);
                await _games.Add(gameData);
                result.Add(_mapper.Map<GameDto>(gameData));
            }
            catch {
                break;
            }
        }

        result.AddRange(_mapper.Map<List<GameDto>>(gamesFromDb));

        return result;
    }
}
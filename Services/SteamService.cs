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

    public SteamService(IConfiguration configuration, IMapper mapper, IGameRepository games) {
        _steamKey = configuration["SteamApiKey"];
        _mapper = mapper;
        _games = games;
    }

    public async Task<List<GameDto>> GetMatchedGames(IEnumerable<string> playersIds) {
        var playersInfo = await GetPlayersInfoByIds(playersIds);
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
        
        return gamesInfo;
    }

    public async Task<List<GameDto>> GetGames(IEnumerable<string> gameAppIds) {
        var gameInfoList = await GetGameInfoFromApiIfRequired(gameAppIds);
        var matchedGames = gameInfoList.Where(x => x != null &&
                                                   x.IsOk &&
                                                   !String.IsNullOrEmpty(x.Tags) &&
                                                   (x.Tags.Contains("Multi-player, , ") ||
                                                    x.Tags.Contains("Co-op") ||
                                                    x.Tags.Contains("Online Co-op")))
            .ToList();

        return matchedGames;
    }

    public async Task<List<string>> GetPlayerIds(IEnumerable<string> playersUrls) {
        using var httpClient = new HttpClient();

        var playerVanityUrls = playersUrls.Select(x => x.Split("/")
            .Last(urlParts => !string.IsNullOrEmpty(urlParts)));
        
        var result = await Task.WhenAll(playerVanityUrls.Select(async x => await GetIdFromSteamApi(x)).ToArray());

        return result.ToList();
    }

    public async Task<List<PlayerDto>> GetPlayersInfoByIds(IEnumerable<string> userIds) {
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

    private async Task<string> GetIdFromSteamApi(string vanityUrl) {
        if (vanityUrl.Length == 17 && vanityUrl.All(char.IsDigit)) {
            return vanityUrl;
        }

        var response = await new HttpClient().GetAsync($"http://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/?key={_steamKey}&vanityurl={vanityUrl}");
        var playersIdsResponseResult = await response.Content.ReadAsStringAsync();
        // todo: if no match?
        var id = JObject.Parse(playersIdsResponseResult)["response"]?["steamid"]?.Value<string>();
        return id;
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
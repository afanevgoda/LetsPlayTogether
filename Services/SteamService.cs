using AutoMapper;
using DataAccess.Repositories;
using LetsPlayTogether.Models;
using LetsPlayTogether.Models.DTO;
using LetsPlayTogether.Models.Steam;
using LetsPlayTogether.Models.Steam.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LetsPlayTogether.Services;

public class SteamService : ISteamService{
    private string _steamKey = "645EA8586FBD3628FF6A01A9338128BB";
    private readonly IMapper _mapper;
    private readonly IGameRepository _games;

    public SteamService(IMapper mapper, IGameRepository games) {
        _mapper = mapper;
        _games = games;
    }

    public async Task<List<Game>> GetMatchedGames(List<string> userIds) {
        var result = await GetPlayersInfo(userIds);

        var appIdsTotal = result.SelectMany(x => x.OwnedAppIds);
        foreach (var player in result) {
            appIdsTotal = appIdsTotal.Intersect(player.OwnedAppIds).ToList();
        }

        return await GetGames(appIdsTotal.ToList());
    }

    public async Task<List<Game>> GetGames(List<string> gameAppIds) {
        var matchedGames = new List<Game>();

        foreach (var appId in gameAppIds) {
            var gameInfo = await GetGameInfoFromApiIfRequired(appId);
            
            if(!gameInfo.IsOk || string.IsNullOrEmpty(gameInfo.Tags))
                continue;

            if (gameInfo.Tags.Contains("Multi-player, , ") ||
                gameInfo.Tags.Contains("Co-op") ||
                gameInfo.Tags.Contains("Online Co-op"))
                matchedGames.Add(gameInfo);
        }

        return matchedGames;
    }
    
    public async Task<List<Player>> GetPlayersInfo(List<string> userIds) {
        using var httpClient = new HttpClient();

        var parameters = string.Join(",", userIds);

        var result = await httpClient.GetAsync(
            $"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={_steamKey}&SteamIds={parameters}");

        var playersResponseResult = await result.Content.ReadAsStringAsync();
        var steamPlayer = JObject.Parse(playersResponseResult)["response"]?["players"]?
            .Select(x => x.ToObject<SteamPlayer>()).ToList();
        var players = _mapper.Map<List<Player>>(steamPlayer);

        foreach (var player in players) {
            player.OwnedAppIds = await GetListOfOwnedAppIds(player.Id);
        }

        return players;
    }

    public async Task<Game> GetGameInfo(string appId) {
        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync($"https://store.steampowered.com/api/appdetails?appids={appId}");
        var gameString = await response.Content.ReadAsStringAsync();
        var desGame = JsonConvert.DeserializeObject<Dictionary<string, AppDetails>>(gameString);
        var appDetails = desGame?[appId];

        if (appDetails?.Data == null)
            return new Game {
                AppId = appId,
                IsOk = false
            };

        var game = _mapper.Map<Game>(appDetails);
        return game;
    }

    private async Task<List<string>> GetListOfOwnedAppIds(string userId) {
        using var httpClient = new HttpClient();
        var gamesResponse = await httpClient.GetAsync(
            $"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={_steamKey}&steamid={userId}&format=json");
        var stringified = await gamesResponse.Content.ReadAsStringAsync();
        var desGames = JsonConvert.DeserializeObject<GetOwnedGames>(stringified);
        return desGames?.Response.Games.Select(x => x.AppId).ToList() ?? new List<string>();
    }

    private async Task<Game> GetGameInfoFromApiIfRequired(string appId) {
        Game result;

        var gameFromDb = await _games.GetByAppId(appId);

        if (gameFromDb == null) {
            result = await GetGameInfo(appId);
            var gameData = _mapper.Map<DataAccess.Models.Game>(result);
            await _games.Add(gameData);
        }
        else result = _mapper.Map<Game>(gameFromDb);

        return result;
    }
}
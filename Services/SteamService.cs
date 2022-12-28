using AutoMapper;
using LetsPlayTogether.Models;
using LetsPlayTogether.Models.Steam;
using LetsPlayTogether.Models.Steam.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LetsPlayTogether.Services;

public class SteamService : ISteamService{
    private string _steamKey = "645EA8586FBD3628FF6A01A9338128BB";
    private readonly IMapper _mapper;

    public SteamService(IMapper mapper) {
        _mapper = mapper;
    }

    public async Task<List<Game>> GetMatchedGames(List<string> userIds) {
        var result = await GetPlayersInfo(userIds);
        
        var appIdsTotal = result.SelectMany(x => x.OwnedAppIds);
        foreach (var player in result) {
            appIdsTotal = appIdsTotal.Intersect(player.OwnedAppIds).ToList();
        }

        var matchedGames = new List<Game>();

        var rand = new Random();
        foreach (var appId in appIdsTotal.OrderBy(x => rand.Next(0, appIdsTotal.Count())).Take(3)) {
            var gameInfo = await GetGameInfo(appId);
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
        var game = _mapper.Map<Game>(appDetails);
    
        return game;
    }

    private async Task<List<string>> GetListOfOwnedAppIds(string userId) {
        using var httpClient = new HttpClient();
        var gamesResponse = await httpClient.GetAsync(
            $"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={_steamKey}&steamid={userId}&format=json");
        var desGames = JsonConvert.DeserializeObject<GetOwnedGames>(await gamesResponse.Content.ReadAsStringAsync());
        return _mapper.Map<List<Game>>(desGames).Select(x => x.Id).ToList();
    }
}
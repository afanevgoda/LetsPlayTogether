using DataAccess.Repositories;
using LetsPlayTogether.Models;
using LetsPlayTogether.Models.DTO;
using LetsPlayTogether.Services;
using Microsoft.AspNetCore.Mvc;

namespace LetsPlayTogether.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController  : ControllerBase{
    private ISteamService _steamService;
    private readonly IGameRepository _games;

    public GamesController(ISteamService steamService, IGameRepository games) {
        this._steamService = steamService;
        this._games = games;
    }
    
    [HttpGet]
    public async Task<List<Game>> GetMatchedGames([FromQuery]List<string> playerUrls) {
        return await _steamService.GetMatchedGames(playerUrls);
    }
    
    [HttpGet("[action]")]
    public async Task<List<Game>> GetGames([FromQuery]List<string> gameAppIds) {
        return await _steamService.GetGames(gameAppIds);
    }
    
    [HttpGet("test")]
    public async void Test() {
        var newGame = new DataAccess.Models.Game {
            Name = "testA",
            HeaderImage = "kek",
            IconUrl = "lol"
        };
        await _games.Add(newGame);
    }
}
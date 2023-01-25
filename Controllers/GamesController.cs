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
    private IPollService _pollService;
    private readonly IGameRepository _games;

    public GamesController(ISteamService steamService, IGameRepository games, IPollService pollService) {
        this._steamService = steamService;
        this._pollService = pollService;
        this._games = games;
    }
    
    [HttpGet]
    public async Task<MatchedGameResponse> GetMatchedGames([FromQuery]List<string> playerIds) {
        var result = new MatchedGameResponse();
        var matchedGames = await _steamService.GetMatchedGames(playerIds);
        result.MatchedGames = matchedGames;
        var pollId = await _pollService.CreatePoll(playerIds, matchedGames);
        result.PollId = pollId;
        return result;
    }
    
    [HttpGet("[action]")]
    public async Task<List<GameDto>> GetGames([FromQuery]List<string> gameAppIds) {
        return await _steamService.GetGames(gameAppIds);
    }
}
using LetsPlayTogether.Models.DTO;
using LetsPlayTogether.Services;
using Microsoft.AspNetCore.Mvc;

namespace LetsPlayTogether.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController  : ControllerBase{
    private readonly ISteamService _steamService;
    private readonly IPollService _pollService;
    private readonly ILogger<GamesController> _logger;

    public GamesController(ISteamService steamService, IPollService pollService, ILogger<GamesController> logger) {
        _steamService = steamService;
        _pollService = pollService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<MatchedGameResponse> GetMatchedGames([FromQuery]List<string> playerIds) {
        var result = new MatchedGameResponse();
        var matchedGames = await _steamService.GetMatchedGames(playerIds);
        result.MatchedGames = matchedGames;
        var pollId = await _pollService.CreatePoll(playerIds, matchedGames);

        // todo: better exceptions, better responses if any, better logs
        if (pollId == null) {
            _logger.LogError("Couldn't create new poll");
            throw new Exception("Couldn't create new poll");
        }

        
        
        result.PollId = pollId;
        return result;
    }
    
    [HttpGet("[action]")]
    public async Task<List<GameDto>> GetGames([FromQuery]List<string> gameAppIds) {
        return await _steamService.GetGames(gameAppIds);
    }
}
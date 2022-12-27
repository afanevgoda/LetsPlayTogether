using LetsPlayTogether.Models;
using LetsPlayTogether.Services;
using Microsoft.AspNetCore.Mvc;

namespace LetsPlayTogether.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController  : ControllerBase{
    private ISteamService _steamService;
    
    public GamesController(ISteamService steamService) {
        this._steamService = steamService;
    }
    
    [HttpGet]
    public async Task<List<Game>> GetMatchedGames([FromQuery]List<string> playerUrls) {
        return await _steamService.GetMatchedGames(playerUrls);
    }
}
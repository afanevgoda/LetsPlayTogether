using LetsPlayTogether.Models;
using LetsPlayTogether.Services;
using Microsoft.AspNetCore.Mvc;

namespace LetsPlayTogether.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayersController : ControllerBase{
    private readonly ISteamService _steamService;
    
    public PlayersController(ISteamService steamService) {
        _steamService = steamService;
    }
    
    [HttpGet]
    public async Task<List<Player>> GetPlayersInfo([FromQuery]List<string> playerUrls) {
        return await _steamService.GetPlayersInfo(playerUrls);
    }

}
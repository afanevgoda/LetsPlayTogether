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
    public async Task<List<PlayerDto>> GetPlayersInfo([FromQuery]List<string> playerUrls) {
        var playerIds = await _steamService.GetPlayerIds(playerUrls);
        return await _steamService.GetPlayersInfoByIds(playerIds);
    }

}
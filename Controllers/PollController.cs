using AutoMapper;
using DataAccess.Models;
using LetsPlayTogether.Models.DTO;
using LetsPlayTogether.Services;
using Microsoft.AspNetCore.Mvc;

namespace LetsPlayTogether.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PollController : ControllerBase{
    private readonly IPollService _pollService;
    private readonly IMapper _mapper;

    public PollController(IPollService pollService, IMapper mapper) {
        _pollService = pollService;
        _mapper = mapper;
    }

    // public async Task<string?> CreatePoll([FromBody] CreatePollRequestDto request) {
    //     return await _pollService.CreatePoll(request.PlayersIds, request.GamesIds);
    // }

    [HttpPost]
    public async Task SubmitPoll([FromBody] SubmitPollRequestDto pollRating) {
        await _pollService.SubmitPoll(pollRating);
    }

    [HttpGet]
    public async Task<PollDto> GetPoll(string pollId) {
        var pollFromDb = await _pollService.Get(pollId);
        return _mapper.Map<PollDto>(pollFromDb);
    }
}
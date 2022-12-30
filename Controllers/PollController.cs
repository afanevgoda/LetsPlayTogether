﻿using AutoMapper;
using DataAccess.Models;
using LetsPlayTogether.Models.DTO;
using LetsPlayTogether.Services;
using Microsoft.AspNetCore.Mvc;
using Poll = LetsPlayTogether.Models.DTO.Poll;

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

    public async Task<string?> CreatePoll([FromBody]CreatePollRequest request) {
        return await _pollService.CreatePoll(request.PlayersIds, request.GamesIds);
    }
    
    [HttpPost]
    public async Task SubmitPoll([FromBody] SubmitPollRequest pollRating) {
        await _pollService.SubmitPoll(pollRating);
    }
    
    [HttpGet]
    public async Task<Poll> GetPoll(string pollId) {
        var pollFromDb =  await _pollService.Get(pollId);
        return _mapper.Map<Poll>(pollFromDb);
    }
}
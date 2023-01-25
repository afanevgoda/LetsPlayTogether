using AutoMapper;
using DataAccess.Models;
using DataAccess.Repositories;
using LetsPlayTogether.Models.DTO;
using PollAppVotes = DataAccess.Models.PollAppVotes;
using ResultVotes = DataAccess.Models.ResultVotes;
using PollMatchedGame = DataAccess.Models.PollMatchedGame;

namespace LetsPlayTogether.Services;

public class PollService : IPollService{
    private readonly IRepository<DataAccess.Models.Poll> _polls;
    private readonly IGameRepository _games;
    private readonly IMapper _mapper;

    public PollService(IRepository<Poll> polls, IMapper mapper, IGameRepository games) {
        _polls = polls;
        _mapper = mapper;
        _games = games;
    }

    public async Task<string?> CreatePoll(List<string> playersIds, IEnumerable<GameDto> games) {
        var poll = new Poll { 
            Games = _mapper.Map<List<PollMatchedGame>>(games),
            PlayerIds = playersIds
        };
        var pollId = await _polls.Add(poll);

        return pollId;
    }

    public async Task SubmitPoll(SubmitPollRequestDto pollRating) {
        var pollFromDb = await _polls.Get(pollRating.Id);

        if (pollFromDb.Votes == null)
            pollFromDb.Votes = _mapper.Map<List<PollAppVotes>>(pollRating.Votes);
        else
            pollFromDb.Votes.AddRange(_mapper.Map<List<PollAppVotes>>(pollRating.Votes));

        pollFromDb.Results = pollFromDb.Votes.GroupBy(x => x.AppId)
            .Select(x => new ResultVotes {
                Rating = x.Select(x => x.Rating).ToList(),
                AppId = x.Key
            }).ToList();
        await _polls.Update(pollFromDb);
    }

    public async Task<PollDto> Get(string pollId) {
        var poll = _mapper.Map<PollDto>(await _polls.Get(pollId));
        var gamesFullInfo = await _games.GetByAppIdList(poll.Games.Select(x => x.AppId));
        // _mapper.Map<List<PollMatchedGameDto>>(gamesFullInfo);
        poll.Games.ForEach(x => {
            var game = gamesFullInfo.FirstOrDefault(y => y.AppId == x.AppId);
            x.Name = game.Name;
            x.HeaderImage = game.HeaderImage;
        });
        return poll;
    }
}
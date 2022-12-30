using AutoMapper;
using DataAccess.Models;
using DataAccess.Repositories;
using LetsPlayTogether.Models.DTO;
using Poll = LetsPlayTogether.Models.DTO.Poll;
using PollAppVotes = DataAccess.Models.PollAppVotes;
using ResultVotes = DataAccess.Models.ResultVotes;

namespace LetsPlayTogether.Services;

public class PollService : IPollService{
    private readonly IRepository<DataAccess.Models.Poll> _polls;
    private readonly IMapper _mapper;

    public PollService(IRepository<DataAccess.Models.Poll> polls, IMapper mapper) {
        _polls = polls;
        _mapper = mapper;
    }

    public async Task<string?> CreatePoll(List<string> playersIds, List<string> gamesIds) {
        var poll = new DataAccess.Models.Poll {
            GameIds = gamesIds,
            PlayerIds = playersIds
        };
        var pollId = await _polls.Add(poll);

        return pollId;
    }

    public async Task SubmitPoll(SubmitPollRequest pollRating) {
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

    public async Task<Poll> Get(string pollId) {
        return _mapper.Map<Poll>(await _polls.Get(pollId));
    }
}
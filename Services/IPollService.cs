using DataAccess.Models;
using LetsPlayTogether.Models.DTO;
using Poll = LetsPlayTogether.Models.DTO.Poll;

namespace LetsPlayTogether.Services;

public interface IPollService{
    Task<string?> CreatePoll(List<string> playersIds, List<string> gamesIds);
    
    Task SubmitPoll(SubmitPollRequest pollRating);

    Task<Poll> Get(string pollId);
}
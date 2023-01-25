using LetsPlayTogether.Models.DTO;
using LetsPlayTogether.Models.DTO.Polls;

namespace LetsPlayTogether.Services;

public interface IPollService{
    Task<string?> CreatePoll(List<string> playersIds, IEnumerable<GameDto> games);
    
    Task SubmitPoll(SubmitPollRequestDto pollRating);

    Task<PollDto> Get(string pollId);
}
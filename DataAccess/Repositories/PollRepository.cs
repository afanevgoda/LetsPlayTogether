using AutoMapper;
using DataAccess.Models;

namespace DataAccess.Repositories;

public class PollRepository : BaseRepository<Poll>{
    public PollRepository(IMapper mapper) : base(mapper) {
    }
}
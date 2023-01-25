using DataAccess.Models;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Repositories;

public class PollRepository : BaseRepository<Poll>{
    public PollRepository(IConfiguration configuration) : base(configuration) {
    }
}
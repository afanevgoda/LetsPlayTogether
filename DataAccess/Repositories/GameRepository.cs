using AutoMapper;
using DataAccess.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace DataAccess.Repositories;

public class GameRepository : BaseRepository<Game>, IGameRepository{
    public GameRepository(IMapper mapper) : base(mapper) { }
    
    public async Task<Game?> GetByAppId(string appId) {
        var database = GetDatabase();
        var filter = Builders<BsonDocument>.Filter.Eq("appId", appId);
        var result = await database.GetCollection<BsonDocument>($"{nameof(Game).ToLower()}s")
            .FindAsync(filter);
        var element = await result.FirstOrDefaultAsync();
        
        if (element == null)
            return null;
        
        return BsonSerializer.Deserialize<Game>(element);
    }
}
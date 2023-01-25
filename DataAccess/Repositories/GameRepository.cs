using DataAccess.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace DataAccess.Repositories;

public class GameRepository : BaseRepository<Game>, IGameRepository{
    public GameRepository(IConfiguration configuration) : base(configuration) { }
    
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

    public async Task<List<Game>?> GetByAppIdList(IEnumerable<string> appIds) {
        var result = new List<Game>();
        var database = GetDatabase();
        var filter = Builders<BsonDocument>.Filter.In("appId", appIds);
        var cursor = await database.GetCollection<BsonDocument>($"{nameof(Game).ToLower()}s")
            .FindAsync(filter);

        var gamesFromDb = await cursor.ToListAsync();
        foreach (var gameFromDb in gamesFromDb) {
            var game = BsonSerializer.Deserialize<Game>(gameFromDb);
            result.Add(game);
        }
        
        return result;
    }
}
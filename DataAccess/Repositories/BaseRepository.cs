using AutoMapper;
using DataAccess.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace DataAccess.Repositories;

public class BaseRepository<T> : IRepository<T> where T: Model {
    protected readonly IMapper Mapper;

    protected BaseRepository(IMapper mapper) {
        Mapper = mapper;
    }

    protected IMongoDatabase GetDatabase() {
        var mongoConnectionUrl = new MongoUrl("mongodb://localhost:27017");
        var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);
        mongoClientSettings.ClusterConfigurator = cb => {
            cb.Subscribe<CommandStartedEvent>(e => {
                Console.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
            });
        };
        // var client = new MongoClient("mongodb://localhost:27017");
        return new MongoClient(mongoClientSettings).GetDatabase("LetsPlayTogether");
    }
    
    public async Task<T> Get(string id) {
        var database = GetDatabase();
        var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
        var result = await database.GetCollection<BsonDocument>($"{typeof(T).Name.ToLower()}s")
            .FindAsync(filter);
        return BsonSerializer.Deserialize<T>(await result.FirstOrDefaultAsync());
    }

    public List<T> GetList(List<string> ids) {
        throw new NotImplementedException();
    }

    public async Task<string?> Add(T newObject) {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("LetsPlayTogether");
        var doc = newObject.ToBsonDocument();
        await database.GetCollection<BsonDocument>($"{typeof(T).Name.ToLower()}s")
            .InsertOneAsync(doc);
        return doc["_id"]?.ToString();
    }

    public void Delete(string id) {
        throw new NotImplementedException();
    }

    public void UpdateList(List<T> updatedObjects) {
        throw new NotImplementedException();
    }

    public async Task Update(T updatedObject) {
        var database = GetDatabase();
        var doc = updatedObject.ToBsonDocument();
        var filter = Builders<BsonDocument>.Filter.Eq("_id", updatedObject.Id);
        await database.GetCollection<BsonDocument>($"{typeof(T).Name.ToLower()}s")
            .ReplaceOneAsync(filter, doc);
    }

    public void Update(List<T> updatedObjects) {
        throw new NotImplementedException();
    }
}
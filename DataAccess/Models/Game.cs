using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace DataAccess.Models;

[BsonDiscriminator("games")]
public class Game : Model{
    [BsonElement("appId")] public string AppId { get; set; }
    
    [BsonElement("name")] public string Name { get; set; }

    [BsonElement("iconUrl")] public string IconUrl { get; set; }

    [BsonElement("imageUrl")] public string HeaderImage { get; set; }

    [BsonElement("isOk")] public bool IsOk { get; set; }
    
    [BsonElement("tags")] public string Tags { get; set; }
}
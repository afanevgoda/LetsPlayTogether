using MongoDB.Bson.Serialization.Attributes;

namespace DataAccess.Models;

[BsonDiscriminator("games")]
public class Game : Model{
    [BsonElement("appId")] public string AppId { get; set; } = null!;
    
    [BsonElement("name")] public string Name { get; set; } = null!;

    [BsonElement("iconUrl")] public string IconUrl { get; set; } = null!;

    [BsonElement("imageUrl")] public string HeaderImage { get; set; } = null!;

    [BsonElement("isOk")] public bool IsOk { get; set; }
    
    [BsonElement("tags")] public string Tags { get; set; } = null!;
}
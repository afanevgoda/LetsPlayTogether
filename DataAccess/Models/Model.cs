using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccess.Models;

public class Model{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId  Id { get; set; }
}
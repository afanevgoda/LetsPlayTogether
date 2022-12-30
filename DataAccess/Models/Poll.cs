using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccess.Models;

public class Poll : Model{
    [BsonElement("playerIds")] public List<string> PlayerIds { get; set; }

    [BsonElement("gameIds")] public List<string> GameIds { get; set; }

    [BsonElement("votes")] public List<PollAppVotes>? Votes { get; set; }
    
    [BsonElement("results")] public List<ResultVotes> Results { get; set; }
}

public class PollAppVotes{
    public string AppId { get; set; }

    public int Rating { get; set; }
}

public class ResultVotes{
    public string AppId { get; set; }

    public List<int> Rating { get; set; }
}
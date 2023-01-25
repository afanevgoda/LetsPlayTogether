using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccess.Models;

public class Poll : Model{
    [BsonElement("playerIds")] public List<string> PlayerIds { get; set; }

    [BsonElement("games")] public List<PollMatchedGame> Games { get; set; }
    
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

public class PollMatchedGame{
    public string AppId { get; set; }

    public bool IsOk { get; set; }

    public string Tags { get; set; }

    public int NumberOfOwningPlayers { get; set; }

    public List<string>? PlayersThatDontHaveGame { get; set; }
}
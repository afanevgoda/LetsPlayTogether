using MongoDB.Bson.Serialization.Attributes;

namespace DataAccess.Models;

public class Poll : Model{
    [BsonElement("playerIds")] public List<string> PlayerIds { get; set; } = null!;

    [BsonElement("games")] public List<PollMatchedGame> Games { get; set; } = null!;
    
    [BsonElement("votes")] public List<PollAppVotes>? Votes { get; set; }

    [BsonElement("results")] public List<ResultVotes> Results { get; set; } = null!;
}

public class PollAppVotes{
    public string AppId { get; set; } = null!;

    public int Rating { get; set; }
}

public class ResultVotes{
    public string AppId { get; set; } = null!;

    public List<int> Rating { get; set; } = null!;
}

public class PollMatchedGame{
    public string AppId { get; set; } = null!;

    public bool IsOk { get; set; }

    public string Tags { get; set; } = null!;

    public int NumberOfOwningPlayers { get; set; }

    public List<string>? PlayersThatDontHaveGame { get; set; }
}
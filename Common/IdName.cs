using System;
using MongoDB.Bson.Serialization.Attributes;

namespace platychat_dotnet.Common;

public class IdName
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;
}

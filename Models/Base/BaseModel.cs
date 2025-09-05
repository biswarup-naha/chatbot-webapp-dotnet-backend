using System;
using MongoDB.Bson.Serialization.Attributes;
using platychat_dotnet.Common;

namespace platychat_dotnet.Models.Base;

public class BaseModel
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("createdAt")]
    [BsonDateTimeOptions]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    [BsonDateTimeOptions]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public IdName? CreatedBy { get; set; } = null;
    public IdName? UpdatedBy { get; set; } = null;
}

using System;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using platychat_dotnet.Models.Base;

namespace platychat_dotnet.Models;

public class User : BaseModel
{
    [BsonElement("firstName")]
    public required string FirstName { get; set; }

    [BsonElement("lastName")]
    public string? LastName { get; set; }

    [BsonElement("email")]
    public required string Email { get; set; }

    [BsonElement("password")]
    public required string Password { get; set; }

    [BsonElement("chats")]
    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> Chats { get; set; } = [];
}

using System;
using MongoDB;
using MongoDB.Bson.Serialization.Attributes;

namespace platychat_dotnet.Models;

public class User
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("firstName")]
    public required string FirstName { get; set; }

    [BsonElement("lastName")]
    public string? LastName { get; set; }

    [BsonElement("email")]
    public required string Email { get; set; }

    [BsonElement("password")]
    public required string Password { get; set; }

    [BsonElement("confirmPassword")]
    public required string ConfirmPassword { get; set; }

    [BsonElement("chats")]
    public String[]? Chats { get; set; }

    [BsonElement("createdAt")]
    [BsonDateTimeOptions]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    [BsonDateTimeOptions]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

using System;
using platychat_dotnet.Models.Base;

namespace platychat_dotnet.Models;

public class Chat:BaseModel
{
    public string ConversationId { get; set; } = null!; // e.g. user:123 or conv:abc
    public string SenderId { get; set; } = null!; // "user:123" or "bot"
    public string SenderType { get; set; } = null!; // "user" | "bot" | "admin"
    public string Text { get; set; } = null!;
    public bool Delivered { get; set; } = false;
}

public enum SenderType
{
    Bot=1,
    User,
    Admin
}
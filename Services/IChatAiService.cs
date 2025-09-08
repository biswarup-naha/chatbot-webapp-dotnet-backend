using System;

namespace platychat_dotnet.Services;

public interface IChatAiService
{
    public Task<string> GetReplyAsync(string conversationId, string userId, string userMessage);
}

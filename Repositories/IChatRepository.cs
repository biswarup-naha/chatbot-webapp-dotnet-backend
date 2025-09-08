using System;
using platychat_dotnet.Models;

namespace platychat_dotnet.Repositories;

public interface IChatRepository
{
    public Task SaveMessageAsync(Chat message);
    public Task<List<Chat>> GetMessagesByConversationIdAsync(string conversationId, int limit);
}

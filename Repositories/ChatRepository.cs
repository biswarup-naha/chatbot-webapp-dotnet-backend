using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using platychat_dotnet.Models;
using platychat_dotnet.Settings;

namespace platychat_dotnet.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly IMongoCollection<Chat> _chats;
    public ChatRepository(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _chats = db.GetCollection<Chat>(settings.Value.ChatsCollectionName);

        //index for faster retrieval by conversationId and createdAt
        var idx = Builders<Chat>.IndexKeys.Ascending(x => x.ConversationId).Descending(x => x.CreatedAt);
        _chats.Indexes.CreateOne(new CreateIndexModel<Chat>(idx));
    }
    public Task<List<Chat>> GetMessagesByConversationIdAsync(string conversationId, int limit=100)
    {
        var filter=Builders<Chat>.Filter.Eq(c=>c.ConversationId,conversationId);
        return _chats.Find(filter)
            .SortByDescending(c=>c.CreatedAt)
            .Limit(limit)   
            .ToListAsync();
    }

    public Task SaveMessageAsync(Chat message)
    {
        return _chats.InsertOneAsync(message);
    }
}

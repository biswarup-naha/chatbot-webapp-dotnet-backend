using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using platychat_dotnet.Models;
using platychat_dotnet.Settings;

namespace platychat_dotnet.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _users = db.GetCollection<User>(settings.Value.UsersCollectionName);
    }

    public Task CreateAsync(User user) => _users.InsertOneAsync(user);

    public Task DeleteAsync(string id) => _users.DeleteOneAsync(u => u.Id == id);

    public Task<List<User>> GetAllAsync() => _users.Find(_ => true).ToListAsync();

    public Task<User> GetByIdAsync(string id) => _users.Find(u => u.Id == id).FirstOrDefaultAsync();

    public Task UpdateAsync(string id, User user) => _users.FindOneAndReplaceAsync(u => u.Id == id, user);
    public Task<User> GetByEmailAsync(string email) => _users.Find(u => u.Email == email).FirstOrDefaultAsync();
}

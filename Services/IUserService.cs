using System;
using platychat_dotnet.Models;

namespace platychat_dotnet.Services;

public interface IUserService
{
    public Task<List<User>> GetAllUsers();
    public Task<User> GetUserById(string id);
    public Task CreateUser(User user);
    public Task UpdateUser(string id, User user);
    public Task DeleteUser(string id);
    public Task<User> GetUserByEmail(string email);
}

using System;
using platychat_dotnet.Models;

namespace platychat_dotnet.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User> GetByIdAsync(string id);
    Task CreateAsync(User user);
    Task UpdateAsync(string id, User user);
    Task DeleteAsync(string id);
    Task<User> GetByEmailAsync(string email);
}

using System;
using platychat_dotnet.Models;
using platychat_dotnet.Repositories;

namespace platychat_dotnet.Services;

public class UserService(IUserRepository repo) : IUserService
{
    private readonly IUserRepository _repo = repo;

    public Task CreateUser(User user) => _repo.CreateAsync(user);

    public Task DeleteUser(string id) => _repo.DeleteAsync(id);

    public Task<List<User>> GetAllUsers() => _repo.GetAllAsync();

    public Task<User> GetUserById(string id) => _repo.GetByIdAsync(id);

    public Task UpdateUser(string id, User user) => _repo.UpdateAsync(id, user);
    public Task<User> GetUserByEmail(string email) => _repo.GetByEmailAsync(email);
}

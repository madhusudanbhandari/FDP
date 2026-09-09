using FDP.Models;

namespace FDP.Interface;

public interface IAuthRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int id);

    Task AddAsync(User user);
    Task SaveChangesAsync();
}

using FDP.Data;
using FDP.Interface;
using FDP.Models;
using Microsoft.EntityFrameworkCore;

namespace FDP.Repository;

public class AuthRepository:IAuthRepository
{
    private readonly AppDbContext _context;
    public AuthRepository(AppDbContext context)
    {
        _context=context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var user=await _context.Users
                .FirstOrDefaultAsync(u=>u.Email==email);

        return user;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var user=await _context.Users
                .FirstOrDefaultAsync(u=>u.Id==id);

        return user;
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }


}
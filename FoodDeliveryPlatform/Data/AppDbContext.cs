
using FDP.Models;
using Microsoft.EntityFrameworkCore;

namespace FDP.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<User>Users{get;set;}
}
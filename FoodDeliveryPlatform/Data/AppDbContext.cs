
using FDP.Models;
using Microsoft.EntityFrameworkCore;

namespace FDP.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<User>Users{get;set;}
    public DbSet<Restaurant>Restaurants{get;set;}
    public DbSet<Menu> Menus{get;set;}
    public DbSet<MenuItem> MenuItems{get;set;}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Menu>()
            .HasOne(m=>m.Restaurant)
            .WithOne(r=>r.Menu)
            .HasForeignKey<Menu>(m=>m.RestaurantId);

        modelBuilder.Entity<MenuItem>()
            .HasOne(mi=>mi.Menu)
            .WithMany(m=>m.MenuItems)
            .HasForeignKey(mi=>mi.MenuId);

    }

}



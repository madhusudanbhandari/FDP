
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
    public DbSet<Cart> Carts{get;set;}
    public DbSet<CartItem> CartItems{get;set;}


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
        
        modelBuilder.Entity<Cart>()
            .HasOne(c=>c.User)
            .WithOne()
            .HasForeignKey<Cart>(c=>c.UserId);
        
        modelBuilder.Entity<CartItem>()
            .HasOne(ci=>ci.Cart)
            .WithMany(c=>c.CartItems)
            .HasForeignKey(ci=>ci.CartId);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci=>ci.MenuItem)
            .WithMany()
            .HasForeignKey(ci=>ci.MenuItemId);


    }

}



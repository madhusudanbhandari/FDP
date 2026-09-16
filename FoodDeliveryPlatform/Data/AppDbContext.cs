
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
    public DbSet<Order> Orders{get;set;}
    public DbSet<OrderItem> OrderItems{get;set;}
    public DbSet<Notification> Notifications{get;set;}
    public DbSet<Review> Reviews{get;set;}
    public DbSet<Payment> Payments{get;set;}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Menu>()
            .HasOne(m=>m.Restaurant)
            .WithOne(r=>r.Menu)
            .HasForeignKey<Menu>(m=>m.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MenuItem>()
            .HasOne(mi=>mi.Menu)
            .WithMany(m=>m.MenuItems)
            .HasForeignKey(mi=>mi.MenuId);
        
        modelBuilder.Entity<Cart>()
            .HasOne(c=>c.User)
            .WithOne(u=>u.Cart)
            .HasForeignKey<Cart>(c=>c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<CartItem>()
            .HasOne(ci=>ci.Cart)
            .WithMany(c=>c.CartItems)
            .HasForeignKey(ci=>ci.CartId);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci=>ci.MenuItem)
            .WithMany()
            .HasForeignKey(ci=>ci.MenuItemId);

        modelBuilder.Entity<Order>()
            .HasOne(o=>o.User)
            .WithMany(u=>u.Orders)
            .HasForeignKey(o=>o.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Order>()
            .HasMany(o=>o.OrderItems)
            .WithOne(oi=>oi.Order)
            .HasForeignKey(oi=>oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi=>oi.MenuItem)
            .WithMany()
            .HasForeignKey(oi=>oi.MenuItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CartItem>()
            .HasIndex(ci=>new{ci.CartId,ci.MenuItemId})
            .IsUnique();
        
        modelBuilder.Entity<Order>()
            .HasOne(o=>o.Restaurant)
            .WithMany()
            .HasForeignKey(o=>o.RestaurantId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Notification>()
            .HasOne(n=>n.User)
            .WithMany(n=>n.Notifications)
            .HasForeignKey(n=>n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Review>()
            .HasOne(r=>r.User)
            .WithMany(r=>r.Reviews)
            .HasForeignKey(r=>r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Review>()
            .HasOne(r=>r.Restaurant)
            .WithMany(r=>r.Reviews)
            .HasForeignKey(r=>r.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Review>()
            .HasOne(r=>r.Order)
            .WithMany()
            .HasForeignKey(r=>r.OrderId)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<Review>()
            .HasIndex(r=>new{r.UserId,r.OrderId})
            .IsUnique();

        modelBuilder.Entity<Order>()
            .HasOne(o=>o.Payment)
            .WithOne(p=>p.Order)
            .HasForeignKey<Payment>(p=> p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Payment>()
            .Property(p=>p.Amount)
            .HasPrecision(18,2);
        
        modelBuilder.Entity<Payment>()
            .HasIndex(p=>p.OrderId)
            .IsUnique();
    }

}



using Microsoft.EntityFrameworkCore;
using onlinefood.Entity;
using BCrypt.Net;

namespace onlinefood.Data;

public class FirstRunDbContext : DbContext
{
    public FirstRunDbContext(DbContextOptions<FirstRunDbContext> options): base(options)
    {
    }
    public DbSet<Users> Users { get; set; }
    public DbSet<Categories> Categories { get; set; }
    public DbSet<FoodItems> FoodItems { get; set; }
    public DbSet<Orders> Orders { get; set; }
    public DbSet<OrderDetails> OrderDetails { get; set; }
    public DbSet<Payments> Payments { get; set; }
    public DbSet<Contacts> Contacts { get; set; }
    public DbSet<EmailVerification> EmailVerifications { get; set; }
    public DbSet<CartItems> CartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Users>().HasData(
            new Users
            {
                UserId = 1,
                Name = "Admin",
                Email = "admin@admin.com",
                Password = "$2a$12$vYvdlAR/4cXJBFHs3LsOhuGt75LSUazrHsW7skY/Lc4rr/CsCDfPC",
                Role = "Admin",
                IsVerified = true,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2025, 04, 11), DateTimeKind.Utc)
            }
        );
    }
}

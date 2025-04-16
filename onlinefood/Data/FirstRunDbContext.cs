using System;
using Microsoft.EntityFrameworkCore;
using onlinefood.Entity;
using onlinefood.Services;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Users>().HasData(
            new Users 
            {
                UserId = 1,
                Name = "Admin",
                Email = "admin@admin.com",
                Password = "$2a$11$2XobGaY7WQkTwv06j6mCrOaK5nBy65A24veCbWH.3Omj7JGkTTnqC",
                Role = "Admin",
                IsVerified = true,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2025, 04, 11), DateTimeKind.Utc)
            }
        );
    }
}

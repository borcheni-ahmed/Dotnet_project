
using Microsoft.EntityFrameworkCore;
using Dotnet_Project.Entities.Oltp;

namespace Dotnet_Project.Data.Oltp
{
    public class OltpDbContext : DbContext
    {
        public OltpDbContext(DbContextOptions<OltpDbContext> options)
            : base(options)
        {
        }

        // DbSets pour les entités OLTP
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration de la relation Customer -> Orders
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Index pour améliorer les performances
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.CustomerName);

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.CustomerID);

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderDate);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Seed data pour les utilisateurs (Admin et User par défaut)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "admin",
                    Email = "admin@salesmanagement.com",
                    // Password: Admin@123
                    PasswordHash = "$2a$11$8vJ8qZ9L1FXKx.yZQXZqYO3zWzF8WKYzGvZQXZqYO3zWzF8WKYZG.",
                    Role = "Admin",
                    CreatedAt = DateTime.Now,
                    IsActive = true
                },
                new User
                {
                    UserId = 2,
                    Username = "user",
                    Email = "user@salesmanagement.com",
                    // Password: User@123
                    PasswordHash = "$2a$11$9wK9rA0M2GYLy.zAQYArZP4aXaG9XLZaHwAQYArZP4aXaG9XLZAH.",
                    Role = "User",
                    CreatedAt = DateTime.Now,
                    IsActive = true
                }
            );
        }
    }
}
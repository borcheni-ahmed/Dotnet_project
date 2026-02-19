using Dotnet_Project.Entities.DataWarehouse;
using Microsoft.EntityFrameworkCore;


namespace Dotnet_Project.Data.DataWarehouse
{
    public class DwDbContext : DbContext
    {
        public DwDbContext(DbContextOptions<DwDbContext> options)
            : base(options)
        {
        }

        // DbSets pour les entités Data Warehouse
        public DbSet<FactSales> FactSales { get; set; }
        public DbSet<DimCustomer> DimCustomers { get; set; }
        public DbSet<DimProduct> DimProducts { get; set; }
        public DbSet<DimDate> DimDates { get; set; }
        public DbSet<DimCity> DimCities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration des relations FactSales -> Dimensions
            modelBuilder.Entity<FactSales>()
                .HasOne(f => f.DimDate)
                .WithMany(d => d.FactSales)
                .HasForeignKey(f => f.DateKey)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FactSales>()
                .HasOne(f => f.DimCustomer)
                .WithMany(c => c.FactSales)
                .HasForeignKey(f => f.CustomerKey)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FactSales>()
                .HasOne(f => f.DimProduct)
                .WithMany(p => p.FactSales)
                .HasForeignKey(f => f.ProductKey)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FactSales>()
                .HasOne(f => f.DimCity)
                .WithMany(c => c.FactSales)
                .HasForeignKey(f => f.CityKey)
                .OnDelete(DeleteBehavior.Restrict);

            // Index pour optimiser les requêtes analytiques
            modelBuilder.Entity<FactSales>()
                .HasIndex(f => f.DateKey)
                .HasDatabaseName("IX_FactSales_DateKey");

            modelBuilder.Entity<FactSales>()
                .HasIndex(f => f.CustomerKey)
                .HasDatabaseName("IX_FactSales_CustomerKey");

            modelBuilder.Entity<FactSales>()
                .HasIndex(f => f.ProductKey)
                .HasDatabaseName("IX_FactSales_ProductKey");

            modelBuilder.Entity<FactSales>()
                .HasIndex(f => f.CityKey)
                .HasDatabaseName("IX_FactSales_CityKey");

            // Index composés pour requêtes fréquentes
            modelBuilder.Entity<FactSales>()
                .HasIndex(f => new { f.DateKey, f.CustomerKey })
                .HasDatabaseName("IX_FactSales_Date_Customer");

            modelBuilder.Entity<FactSales>()
                .HasIndex(f => new { f.DateKey, f.ProductKey })
                .HasDatabaseName("IX_FactSales_Date_Product");

            // Index pour les dimensions
            modelBuilder.Entity<DimCustomer>()
                .HasIndex(c => c.CustomerID);

            modelBuilder.Entity<DimCustomer>()
                .HasIndex(c => c.IsCurrent);

            modelBuilder.Entity<DimProduct>()
                .HasIndex(p => p.ProductID);

            modelBuilder.Entity<DimProduct>()
                .HasIndex(p => p.IsCurrent);

            modelBuilder.Entity<DimCity>()
                .HasIndex(c => c.CityID);

            modelBuilder.Entity<DimDate>()
                .HasIndex(d => d.Year);

            modelBuilder.Entity<DimDate>()
                .HasIndex(d => new { d.Year, d.Month });
        }
    }
}
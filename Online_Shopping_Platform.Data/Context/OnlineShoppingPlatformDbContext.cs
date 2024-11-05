using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Online_Shopping_Platform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Shopping_Platform.Data.Context
{
    public class OnlineShoppingPlatformDbContext : DbContext
    {
        // Constructor to pass options for configuring DbContext
        public OnlineShoppingPlatformDbContext(DbContextOptions<OnlineShoppingPlatformDbContext> options) : base(options)
        {

        }

        // Fluent API configuration for model relationships and constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());  // Apply order entity configuration
            modelBuilder.ApplyConfiguration(new ProductConfiguration());  // Apply product entity configuration
            modelBuilder.ApplyConfiguration(new OrderProductConfiguration());  // Apply order-product entity configuration
            modelBuilder.ApplyConfiguration(new UserConfiguration());  // Apply user entity configuration

            // Seed default settings, such as MaintenanceMode flag
            modelBuilder.Entity<SettingEntity>().HasData(
                new SettingEntity
                {
                    Id = 1,
                    MaintenanceMode = false  // Default to false, indicating the platform is not under maintenance
                });

            base.OnModelCreating(modelBuilder);  // Ensure base class configurations are applied
        }

        // Define DbSets for CRUD operations on entities
        public DbSet<UserEntity> Users => Set<UserEntity>();  // Represents the Users table
        public DbSet<OrderEntity> Orders => Set<OrderEntity>();  // Represents the Orders table
        public DbSet<ProductEntity> Products => Set<ProductEntity>();  // Represents the Products table
        public DbSet<OrderProductEntity> OrderProducts => Set<OrderProductEntity>();  // Represents the OrderProducts table
        public DbSet<SettingEntity> Settings { get; set; }  // Represents the Settings table
    }

}

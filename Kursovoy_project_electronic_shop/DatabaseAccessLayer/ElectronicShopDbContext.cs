using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.Metrics;
using System.IO;
using System.Net.Sockets;

namespace DatabaseAccessLayer
{
    public class ElectronicShopDbContext : DbContext
    {
        public ElectronicShopDbContext(DbContextOptions<ElectronicShopDbContext> options) : base(options) 
        { 
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("Users"); // Добавляем это

            builder.Entity<User>().HasKey(x => x.UserId);
            builder.Entity<User>().HasMany(x => x.Orders).WithOne(x => x.User).HasForeignKey("UserId");

            builder.Entity<Product>().HasKey(x => x.ProductId);
            builder.Entity<Product>().HasMany(x => x.Orders).WithOne(x => x.Product).HasForeignKey("ProductId");

            builder.Entity<Order>().HasKey(x => x.OrderId);
            builder.Entity<Order>().HasOne(x => x.User).WithMany(x => x.Orders).HasForeignKey("UserId");

            builder.Entity<Order>().HasOne(x => x.Product).WithMany(x => x.Orders).HasForeignKey("ProductId");

            builder.Entity<Manufacturer>().HasKey(x => x.ManufacturerId);
            builder.Entity<Manufacturer>().HasMany(x => x.Products).WithMany(x => x.Manufacturers)
                .UsingEntity("Product_manufacturer",
                    l => l.HasOne(typeof(Product)).WithMany().HasForeignKey("ProductId").HasPrincipalKey(nameof(Product.ProductId)),
                    r => r.HasOne(typeof(Manufacturer)).WithMany().HasForeignKey("ManufacturerId").HasPrincipalKey(nameof(Manufacturer.ManufacturerId)),
                    j => j.HasKey("ManufacturerId", "ProductId"));

            builder.Entity<Entities.Type>().HasKey(x => x.TypeId);
            builder.Entity<Entities.Type>().HasMany(x => x.Products).WithMany(x => x.Types)
                .UsingEntity("Product_type",
                    l => l.HasOne(typeof(Product)).WithMany().HasForeignKey("ProductId").HasPrincipalKey(nameof(Product.ProductId)),
                    r => r.HasOne(typeof(Entities.Type)).WithMany().HasForeignKey("TypeId").HasPrincipalKey(nameof(Entities.Type.TypeId)),
                    j => j.HasKey("TypeId", "ProductId"));
        }
    }
}

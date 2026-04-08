using cart_service.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cart_service.Persistence.Context
{
    public class CartServiceDbContext : DbContext
    {
        public CartServiceDbContext(DbContextOptions<CartServiceDbContext> options) : base(options) { }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.Property(c => c.TotalAmount)
                    .HasPrecision(18, 2);

                entity.HasMany(c => c.CartItems)
                    .WithOne(ci => ci.Cart)
                    .HasForeignKey(ci => ci.CartId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.Property(ci => ci.UnitPrice)
                    .HasPrecision(18, 2);

                entity.Property(ci => ci.ProductName)
                   .HasMaxLength(200)
                   .IsRequired();

                entity.Property(ci => ci.ProductName)
                   .HasMaxLength(200)
                   .IsRequired();

                entity.HasIndex(ci => new { ci.CartId, ci.ProductVariantId })
                    .IsUnique();
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}

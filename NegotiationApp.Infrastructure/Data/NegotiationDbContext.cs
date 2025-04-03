using Microsoft.EntityFrameworkCore;
using NegotiationApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegotiationApp.Infrastructure.Data
{
    public class NegotiationDbContext : DbContext
    {
        public NegotiationDbContext(DbContextOptions<NegotiationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Negotiation> Negotiations { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Price)
                      .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Negotiation>(entity =>
            {
                entity.Property(e => e.ProposedPrice)
                      .HasColumnType("decimal(18,2)");
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}

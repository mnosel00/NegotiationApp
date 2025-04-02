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


    }
}

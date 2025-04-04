using Microsoft.EntityFrameworkCore;
using NegotiationApp.Domain.Entities;
using NegotiationApp.Domain.Interfaces;
using NegotiationApp.Infrastructure.Data;

namespace NegotiationApp.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly NegotiationDbContext _context;

        public ProductRepository(NegotiationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync() => await _context.Products.ToListAsync();

        public async Task<Product?> GetByIdAsync(int id) => await _context.Products.FindAsync(id);
    }
}

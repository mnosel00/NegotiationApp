using Microsoft.EntityFrameworkCore;
using NegotiationApp.Domain.Entities;
using NegotiationApp.Domain.Interfaces;
using NegotiationApp.Infrastructure.Data;

namespace NegotiationApp.Infrastructure.Repositories
{
    public class NegotiationRepository : INegotiationRepository
    {
        private readonly NegotiationDbContext _context;
        public NegotiationRepository(NegotiationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Negotiation negotiation)
        {
            await _context.Negotiations.AddAsync(negotiation);
            
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Negotiation>> GetAllAsync()
            => await _context.Negotiations.ToListAsync();

        public async Task<Negotiation?> GetByIdAsync(int id)
            => await _context.Negotiations.FindAsync(id);

        public async Task UpdateAsync(Negotiation negotiation)
        {
            _context.Negotiations.Update(negotiation);
            
            await _context.SaveChangesAsync();
        }
    }
}

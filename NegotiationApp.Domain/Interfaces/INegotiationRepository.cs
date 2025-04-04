using NegotiationApp.Domain.Entities;

namespace NegotiationApp.Domain.Interfaces
{
    public interface INegotiationRepository
    {
        Task<IEnumerable<Negotiation>> GetAllAsync();
        Task<Negotiation?> GetByIdAsync(int id);
        Task AddAsync(Negotiation negotiation);
        Task UpdateAsync(Negotiation negotiation);
    }
}

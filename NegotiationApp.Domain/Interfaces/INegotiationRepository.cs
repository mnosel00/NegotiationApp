using NegotiationApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

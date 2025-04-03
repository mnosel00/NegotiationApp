using NegotiationApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegotiationApp.Application.Interfaces
{
    public interface INegotiationService
    {
        Task<IEnumerable<NegotiationDto>> GetAllNegotiationsAsync();
        Task<NegotiationDto?> GetNegotiationByIdAsync(int id);
        Task AddNegotiationAsync(CreateNegotiationDto negotiationDto);
        Task ProposeNewPriceAsync(int id, decimal newPrice);
        Task AcceptNegotiationAsync(int id);
        Task RejectNegotiationAsync(int id);
        Task<TimeSpan> CheckExpirationAsync(int id);
    }
}

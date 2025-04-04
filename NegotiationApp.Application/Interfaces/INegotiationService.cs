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
        Task<TimeSpan> CheckExpirationAsync(int id);
        Task<(bool isSuccess, string errorMessage)> AddNegotiationAsync(CreateNegotiationDto negotiationDto);
        Task<(bool isSuccess, string errorMessage)> ProposeNewPriceAsync(int id, decimal newPrice);
        Task<(bool isSuccess, string errorMessage)> RejectNegotiationAsync(int id);
        Task<(bool isSuccess, string errorMessage)> AcceptNegotiationAsync(int id);
    }
}

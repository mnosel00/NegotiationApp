using NegotiationApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NegotiationApp.Domain.Entities.Negotiation;

namespace NegotiationApp.Application.DTOs
{
    public class NegotiationDto
    {
        public int ProductId { get; }
        public decimal ProposedPrice { get; }
        public DateTime ProposedAt { get; }
        public int Attempts { get; }
        public NegotiationStatus Status { get; }

        public NegotiationDto(int productId, decimal proposedPrice, DateTime proposedAt, int attempts, NegotiationStatus status)
        {
            ProductId = productId;
            ProposedPrice = proposedPrice;
            ProposedAt = proposedAt;
            Attempts = attempts;
            Status = status;
        }
    }
}

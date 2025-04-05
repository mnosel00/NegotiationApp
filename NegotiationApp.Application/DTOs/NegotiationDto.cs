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
        public int ProductId { get; private set; }
        public decimal ProposedPrice { get; private set; }
        public DateTime ProposedAt { get; private set; }
        public int Attempts { get; private set; }
        public string Status { get; private set; }

        public NegotiationDto(int productId, decimal proposedPrice, DateTime proposedAt, int attempts, string status)
        {
            ProductId = productId;
            ProposedPrice = proposedPrice;
            ProposedAt = proposedAt;
            Attempts = attempts;
            Status = status;
        }
    }
}

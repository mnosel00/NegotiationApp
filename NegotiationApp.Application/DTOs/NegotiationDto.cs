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
        private const int MaxAttempts = 3;
        private const int ExpirationDays = 7;

        public int ProductId { get; }
        public decimal ProposedPrice { get; private set; }
        public DateTime ProposedAt { get; private set; }
        public int Attempts { get; private set; }
        public NegotiationStatus Status { get; private set; }

        public NegotiationDto(int productId, decimal proposedPrice)
        {
            if (proposedPrice <= 0)
                throw new ArgumentException("Proposed price must be greater than zero.");

            ProductId = productId;
            ProposedPrice = proposedPrice;
            ProposedAt = DateTime.UtcNow;
            Attempts = 1;
            Status = NegotiationStatus.Pending;
        }

        public void ProposeNewPrice(decimal newPrice)
        {
            if (Status != NegotiationStatus.Pending)
                throw new InvalidOperationException("Cannot propose a new price on a closed negotiation.");

            if (Attempts >= MaxAttempts)
                throw new InvalidOperationException("Maximum negotiation attempts reached.");

            if (newPrice <= 0)
                throw new ArgumentException("New price must be greater than zero.");

            ProposedPrice = newPrice;
            ProposedAt = DateTime.UtcNow;
            Attempts++;
        }

        public void Accept()
        {
            if (Status != NegotiationStatus.Pending)
                throw new InvalidOperationException("Only pending negotiations can be accepted.");

            Status = NegotiationStatus.Accepted;
        }

        public void Reject()
        {
            if (Status != NegotiationStatus.Pending)
                throw new InvalidOperationException("Only pending negotiations can be rejected.");

            if (Attempts >= MaxAttempts)
            {
                Status = NegotiationStatus.Expired;
            }
            else
            {
                Status = NegotiationStatus.Rejected;
            }
        }

        public void CheckExpiration()
        {
            if (Status == NegotiationStatus.Pending && (DateTime.UtcNow - ProposedAt).TotalDays > ExpirationDays)
            {
                Status = NegotiationStatus.Expired;
            }
        }
    }
}

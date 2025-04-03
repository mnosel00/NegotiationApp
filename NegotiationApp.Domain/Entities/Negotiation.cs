using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegotiationApp.Domain.Entities
{
    public class Negotiation
    {
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public decimal ProposedPrice { get; private set; }
        public DateTime ProposedAt { get; private set; }
        public int Attempts { get; private set; }
        public NegotiationStatus Status { get; private set; }

        private Negotiation() { }
        public Negotiation(int productId, decimal proposedPrice)
        {
            ProductId = productId;
            ProposedPrice = proposedPrice;
            ProposedAt = DateTime.UtcNow;
            Attempts = 1;
            Status = NegotiationStatus.Pending;
        }

        public void UpdateProposedPrice(decimal newPrice)
        {
            ProposedPrice = newPrice;
            ProposedAt = DateTime.UtcNow;
            Attempts++;
        }

        public void SetStatus(NegotiationStatus status)
        {
            Status = status;
        }


        public enum NegotiationStatus
        {
            Pending,
            Accepted,
            Rejected,
            Expired
        }
    }
}

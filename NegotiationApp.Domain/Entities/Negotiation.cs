using NegotiationApp.Domain.Enums;
using System.Diagnostics;

namespace NegotiationApp.Domain.Entities
{
    public class Negotiation
    {
        private const int MaxAttempts = 3;

        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public decimal ProposedPrice { get; private set; }
        public DateTime ProposedAt { get; private set; }
        public int Attempts { get; private set; }
        public NegotiationStatus Status { get; private set; }

        public Negotiation(int productId, decimal proposedPrice)
        {
            if (proposedPrice <= 0)
                throw new ArgumentException("Proposed price must be greater than zero.", nameof(proposedPrice));

            ProductId = productId;
            ProposedPrice = proposedPrice;
            ProposedAt = DateTime.UtcNow;
            Attempts = 1;
            Status = NegotiationStatus.Pending;
        }

        public void ProposeNewPrice(decimal newPrice)
        {
            if (Status == NegotiationStatus.Expired)
                throw new InvalidOperationException("Cannot propose a new price on an expired negotiation.");

            if (Attempts >= MaxAttempts)
                throw new InvalidOperationException("Maximum negotiation attempts reached.");

            if (newPrice <= 0)
                throw new ArgumentException("New price must be greater than zero.", nameof(newPrice));

            ProposedPrice = newPrice;
            ProposedAt = DateTime.UtcNow;
            Attempts++;
            Status = NegotiationStatus.Pending;
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
                Status = NegotiationStatus.Expired;
            else
                Status = NegotiationStatus.Rejected;
            
        }

        public TimeSpan CheckExpiration()
        {
            var expirationTime = ProposedAt.AddDays(7);
            var timeRemaining = expirationTime - DateTime.UtcNow;

            Debug.WriteLine($"ProposedAt: {ProposedAt}");
            Debug.WriteLine($"ExpirationTime: {expirationTime}");
            Debug.WriteLine($"TimeRemaining: {timeRemaining}");

            if (timeRemaining <= TimeSpan.Zero)
            {
                Expire();
                Debug.WriteLine("Negotiation expired.");
                return TimeSpan.Zero;
            }

            return timeRemaining;
        }
        public void Expire()
        {
            Status = NegotiationStatus.Expired;
            Debug.WriteLine("Status set to Expired.");

        }
    }

   
}


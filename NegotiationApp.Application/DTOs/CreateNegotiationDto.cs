using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegotiationApp.Application.DTOs
{
    public class CreateNegotiationDto
    {
        public int ProductId { get; }
        public decimal ProposedPrice { get; }

        public CreateNegotiationDto(int productId, decimal proposedPrice)
        {
            ProductId = productId;
            ProposedPrice = proposedPrice;
        }
    }
}

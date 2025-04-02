using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NegotiationApp.Application.DTOs
{
    public class ProductDto
    {
        public string Name { get; }
        public decimal Price { get; }

        public ProductDto(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(price));

            Name = name;
            Price = price;
        }
    }
}

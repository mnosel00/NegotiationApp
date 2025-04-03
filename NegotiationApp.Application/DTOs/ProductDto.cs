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
            Name = name;
            Price = price;
        }
    }
}

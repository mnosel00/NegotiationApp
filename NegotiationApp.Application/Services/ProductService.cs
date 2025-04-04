using NegotiationApp.Application.DTOs;
using NegotiationApp.Application.Interfaces;
using NegotiationApp.Domain.Entities;
using NegotiationApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegotiationApp.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<(bool isSuccess, string errorMessage)> AddProductAsync(ProductDto productDto)
        {
            try
            {
                var product = new Product(productDto.Name, productDto.Price);
                await _productRepository.AddAsync(product);
                return (true, string.Empty);
            }
            catch (ArgumentException ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => new ProductDto(p.Name, p.Price));
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return null;
            }
            return new ProductDto(product.Name, product.Price);
        }
    }
}

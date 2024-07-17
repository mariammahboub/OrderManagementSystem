using OrderManagement.Core.DTOs;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagement.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;

        public ProductService(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        #region GetAllProductsAsync
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock
            });
        } 
        #endregion

        #region GetProductByIdAsync
        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };
        }
        #endregion

        #region CreateProductAsync
        public async Task<int> CreateProductAsync(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Stock = productDto.Stock
            };

            await _productRepository.AddAsync(product);
            return product.Id;
        }

        #endregion

        #region UpdateProductAsync
        public async Task<bool> UpdateProductAsync(ProductDto productDto)
        {
            var product = await _productRepository.GetByIdAsync(productDto.Id);
            if (product == null) return false;
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Stock = productDto.Stock;
            await _productRepository.UpdateAsync(product);
            return true;
        }
        #endregion

        #region DeleteProductAsync
        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return false;

            await _productRepository.DeleteAsync(productId);
            return true;
        } 
        #endregion
    }
}

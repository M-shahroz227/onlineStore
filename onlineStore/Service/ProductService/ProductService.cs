using onlineStore.Data;
using onlineStore.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using onlineStore.DTO.ProductDto;
using Microsoft.EntityFrameworkCore;

namespace onlineStore.Service.ProductService
{
    public class ProductService : IProductService
    {
        private readonly StoreDbContext _context;
        public ProductService(StoreDbContext context) 
        {
            _context = context;
            

        }
        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var products = await _context.products.ToListAsync();
            var productDtos = new List<ProductDto>();
            foreach (var product in products)
            {
                productDtos.Add(new ProductDto
                {
                    Id = product.Id,
                    Tile = product.Tile,
                    Price = product.Price,
                    Stock = product.Stock,
                    Description = product.Description
                });
            }
            return productDtos;
        }
        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _context.products.FindAsync(id);
            if (product == null)
            {
                return null;
            }
            return new ProductDto
            {
                Id = product.Id,
                Tile = product.Tile,
                Price = product.Price,
                Stock = product.Stock,
                Description = product.Description
            };
        }
        public async Task<ProductDto> AddProductAsync(ProductDto productDto)
        {
            var product = new Product
            {
                Tile = productDto.Tile,
                Price = productDto.Price,
                Stock = productDto.Stock,
                Description = productDto.Description
            };
            _context.products.Add(product);
            await _context.SaveChangesAsync();
            productDto.Id = product.Id;
            return productDto;
        }
        public async Task<ProductDto> UpdateProductAsync(int id, ProductDto updatedProductDto)
        {
            var product = await _context.products.FindAsync(id);
            if (product == null)
            {
                return null;
            }
            product.Tile = updatedProductDto.Tile;
            product.Price = updatedProductDto.Price;
            product.Stock = updatedProductDto.Stock;
            product.Description = updatedProductDto.Description;
            await _context.SaveChangesAsync();
            return updatedProductDto;
        }
        public async Task<ProductDto> GetProductByIdAsync(string productId)
        {
            if (!int.TryParse(productId, out int id))
            {
                return null;
            }
            return await GetProductByIdAsync(id);
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.products.FindAsync(id);
            if (product == null)
            {
                return false;
            }
            _context.products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }


        
    }
}

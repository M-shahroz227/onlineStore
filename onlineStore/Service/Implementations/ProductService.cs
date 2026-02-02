using Microsoft.EntityFrameworkCore;
using onlineStore.Data;
using onlineStore.DTO.ProductDto;
using onlineStore.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace onlineStore.Service.ProductService
{
    public class ProductService : IProductService
    {
        private readonly StoreDbContext _context;

        public ProductService(StoreDbContext context)
        {
            _context = context;
        }

        // -------------------- GET ALL --------------------
        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            return await _context.Products
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Tile = p.Title,
                    Price = p.Price,
                    Stock = p.Stock,
                    Description = p.Description
                })
                .ToListAsync();
        }

        // -------------------- GET BY ID --------------------
        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Tile = product.Title,
                Price = product.Price,
                Stock = product.Stock,
                Description = product.Description
            };
        }

        // -------------------- ADD --------------------
        public async Task<ProductDto> AddProductAsync(ProductDto productDto)
        {
            var product = new Product
            {
                Title = productDto.Tile,
                Price = productDto.Price,
                Stock = productDto.Stock,
                Description = productDto.Description
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            productDto.Id = product.Id;
            return productDto;
        }

        // -------------------- UPDATE --------------------
        public async Task<ProductDto> UpdateProductAsync(int id, ProductDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            product.Title = productDto.Tile;
            product.Price = productDto.Price;
            product.Stock = productDto.Stock;
            product.Description = productDto.Description;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            productDto.Id = product.Id;
            return productDto;
        }

        // -------------------- DELETE --------------------
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

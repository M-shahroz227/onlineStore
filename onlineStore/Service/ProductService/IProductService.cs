
using onlineStore.DTO.ProductDto;

namespace onlineStore.Service.ProductService
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<ProductDto> GetProductByIdAsync(string productId);
        Task<ProductDto> AddProductAsync(ProductDto productDto);
        Task<ProductDto> UpdateProductAsync(int id, ProductDto updatedProductDto);
        Task<ProductDto> DeleteProductAsync(int id);

    }
}

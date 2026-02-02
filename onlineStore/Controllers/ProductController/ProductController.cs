using Microsoft.AspNetCore.Mvc;
using onlineStore.DTO.ProductDto;
using onlineStore.Service.ProductService;
using onlineStore.Authorization;
using onlineStore.Common;

namespace onlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // -------------------- READ ALL --------------------
        [FeatureAuthorize(AppFeatures.PRODUCT_VIEW)]
        [HttpGet("getAll")]
        public async Task<ActionResult<List<ProductDto>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        // -------------------- READ BY ID --------------------
        [FeatureAuthorize(AppFeatures.PRODUCT_VIEW)]
        [HttpGet("getById/{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new { message = "Product not found" });

            return Ok(product);
        }

        // -------------------- CREATE --------------------
        [FeatureAuthorize(AppFeatures.PRODUCT_CREATE)]
        [HttpPost("add")]
        public async Task<ActionResult<ProductDto>> AddProduct([FromBody] ProductDto productDto)
        {
            if (productDto == null)
                return BadRequest();

            var createdProduct = await _productService.AddProductAsync(productDto);

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = createdProduct.Id },
                createdProduct
            );
        }

        // -------------------- UPDATE --------------------
        [FeatureAuthorize(AppFeatures.PRODUCT_UPDATE)] // ✅ fix here
        [HttpPut("update/{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, productDto);

            if (updatedProduct == null)
                return NotFound(new { message = "Product not found" });

            return Ok(updatedProduct);
        }

        // -------------------- DELETE --------------------
        [FeatureAuthorize(AppFeatures.PRODUCT_DELETE)]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var isDeleted = await _productService.DeleteProductAsync(id);

            if (!isDeleted)
                return NotFound(new { message = "Product not found" });

            return NoContent(); // 204
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using onlineStore.Service.ProductService;
using onlineStore.DTO.ProductDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace onlineStore.Controllers.ProductController
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
        // GET: api/Product/getAll
        [HttpGet("getAll")]
        public async Task<ActionResult<List<ProductDto>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        // -------------------- READ BY ID --------------------
        // GET: api/Product/getById/1
        [HttpGet("getById/{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new { message = "Product not found" });

            return Ok(product);
        }

        // -------------------- CREATE --------------------
        // POST: api/Product/add
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
        // PUT: api/Product/update/1
        [HttpPut("update/{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, productDto);

            if (updatedProduct == null)
                return NotFound(new { message = "Product not found" });

            return Ok(updatedProduct);
        }

        // -------------------- DELETE --------------------
        // DELETE: api/Product/delete/1
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

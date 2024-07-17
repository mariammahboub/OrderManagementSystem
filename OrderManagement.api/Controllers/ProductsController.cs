using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Core.DTOs;
using OrderManagement.Core.Interfaces;

namespace OrderManagement.api.Controllers
{
    public class ProductsController : ApiBaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        #region GetAllProducts
        // GET /api/products
        [HttpGet]

        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        #endregion

        #region GetProductById
        // GET /api/products/{productId}
        [HttpGet("{productId}")]

        public async Task<ActionResult<ProductDto>> GetProductById(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        #endregion

        #region CreateProduct
        // POST /api/products (admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> CreateProduct([FromBody] ProductDto productDto)
        {
            var productId = await _productService.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(GetProductById), new { productId }, productId);
        }
        #endregion

        #region UpdateProduct
        // PUT /api/products/{productId} (admin only)
        [HttpPut("{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateProduct(int productId, [FromBody] ProductDto productDto)
        {
            // Log incoming values
            Console.WriteLine($"ProductId from URL: {productId}");
            Console.WriteLine($"ProductId from DTO: {productDto.Id}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (productId != productDto.Id)
            {
                return BadRequest("Product ID mismatch.");
            }

            var result = await _productService.UpdateProductAsync(productDto);
            if (!result)
            {
                return NotFound("Product not found.");
            }

            return NoContent();
        } 
        #endregion


    }
}

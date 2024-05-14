using DLS_Catalog_Service.Model;
using DLS_Catalog_Service.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DLS_Catalog_Service.Controllers
{
    [Route("api/productdetails")] // Attribute route for the controller
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {
        private readonly IProductDetailRepository _productDetailRepository;

        public ProductDetailsController(IProductDetailRepository productDetailRepository)
        {
            _productDetailRepository = productDetailRepository;
        }

        [HttpGet] // Attribute route for the action method
        public async Task<IActionResult> GetAllProductDetails()
        {
            var productDetails = await _productDetailRepository.GetAllAsync();
            return Ok(productDetails);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductDetailById(string id)
        {
            var productDetail = await _productDetailRepository.GetByIdAsync(id);
            if (productDetail == null) return NotFound();
            return Ok(productDetail);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductDetail([FromBody] ProductDetail productDetail)
        {
            var createdProductDetail = await _productDetailRepository.CreateAsync(productDetail);
            return CreatedAtAction(nameof(GetProductDetailById), new { id = createdProductDetail.Id },
                createdProductDetail);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductDetail(string id, [FromBody] ProductDetail productDetail)
        {
            if (id != productDetail.Id) return BadRequest();
            var updated = await _productDetailRepository.UpdateAsync(id, productDetail);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductDetail(string id)
        {
            var deleted = await _productDetailRepository.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tesisatci.Data;
using Tesisatci.Dtos;
using Tesisatci.Models;
using Tesisatci.Services;

namespace Tesisatci.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly TesisatciDbContext _context;
        private readonly PhotoService _photoService;

        public ProductController(TesisatciDbContext context, PhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        // GET: api/product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync(); // Images include kaldırıldı
        }

        // GET: api/product/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id); // Images include kaldırıldı

            if (product == null)
                return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromForm] ProductCreateDto dto)
        {
            var result = await _photoService.AddPhotoAsync(dto.Image);
            if (result.Error != null)
                return BadRequest(result.Error.Message);

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Unit = dto.Unit,
                Price = dto.Price,
                CostPrice = dto.CostPrice,
                Stock = dto.Stock,
                CriticalStock = dto.CriticalStock,
                CategoryId = dto.CategoryId,
                ImageUrl = result.SecureUrl.ToString(), // ⭐️ EKLENDİ
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }





        // PUT: api/product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product updatedProduct)
        {
            if (id != updatedProduct.Id)
                return BadRequest();

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.CostPrice = updatedProduct.CostPrice;
            product.Stock = updatedProduct.Stock;
            product.CriticalStock = updatedProduct.CriticalStock;
            product.IsActive = updatedProduct.IsActive;
            product.ShowPrice = updatedProduct.ShowPrice;
            product.CategoryId = updatedProduct.CategoryId;
           
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{productId}/images")]
        public async Task<IActionResult> AddProductImage(int productId, IFormFile file)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);

            var image = new ProductImage
            {
                ProductId = productId,
                Url = result.SecureUrl.ToString()
            };

            _context.ProductImages.Add(image);
            await _context.SaveChangesAsync();

            return Ok(image);
        }
    }
}

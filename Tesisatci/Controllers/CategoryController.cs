using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tesisatci.Data;
using Tesisatci.Dtos;
using Tesisatci.Models;

namespace Tesisatci.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly TesisatciDbContext _context;

        public CategoryController(TesisatciDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            return category;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(CategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                ParentCategoryId = dto.ParentCategoryId
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto updatedCategory)
        {
            if (id != updatedCategory.Id)
                return BadRequest();

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            category.Name = updatedCategory.Name;
            category.ParentCategoryId = updatedCategory.ParentCategoryId == 0 ? null : updatedCategory.ParentCategoryId;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("tree")]
        public async Task<ActionResult<IEnumerable<CategoryTreeDto>>> GetCategoryTree()
        {
            var categories = await _context.Categories.ToListAsync();

            var tree = categories
                .Where(c => c.ParentCategoryId == null)
                .Select(c => new CategoryTreeDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    SubCategories = GetSubCategories(categories, c.Id)
                }).ToList();

            return tree;
        }

        private List<CategoryTreeDto> GetSubCategories(List<Category> allCategories, int parentId)
        {
            return allCategories
                .Where(c => c.ParentCategoryId == parentId)
                .Select(c => new CategoryTreeDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    SubCategories = GetSubCategories(allCategories, c.Id) // 🔁 recursive
                }).ToList();
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tesisatci.Data;
using Tesisatci.Models;
using Tesisatci.Services;
using Tesisatci.Dtos;


namespace Tesisatci.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveredWorksController : ControllerBase
    {
        private readonly TesisatciDbContext _context;
        private readonly PhotoService _photoService;

        public DeliveredWorksController(TesisatciDbContext context, PhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveredWork>>> GetDeliveredWorks()
        {
            return await _context.DeliveredWorks.OrderByDescending(d => d.CreatedAt).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveredWork>> GetDeliveredWork(int id)
        {
            var work = await _context.DeliveredWorks.FindAsync(id);
            if (work == null)
                return NotFound();

            return work;
        }

        [HttpPost]
        public async Task<ActionResult<DeliveredWork>> CreateDeliveredWork([FromForm] DeliveredWorkDto dto)
        {
            var result = await _photoService.AddPhotoAsync(dto.Image);
            if (result.Error != null)
                return BadRequest(result.Error.Message);

            var work = new DeliveredWork
            {
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = result.SecureUrl.ToString()
            };

            _context.DeliveredWorks.Add(work);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDeliveredWork), new { id = work.Id }, work);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeliveredWork(int id, [FromForm] DeliveredWorkUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var work = await _context.DeliveredWorks.FindAsync(id);
            if (work == null)
                return NotFound();

            // Title update
            if (!string.IsNullOrEmpty(dto.Title))
                work.Title = dto.Title;

            // Description update
            if (!string.IsNullOrEmpty(dto.Description))
                work.Description = dto.Description;

            // Image update (varsa Cloudinary'e yükle)
            if (dto.Image != null)
            {
                var result = await _photoService.AddPhotoAsync(dto.Image);
                if (result.Error != null)
                    return BadRequest(result.Error.Message);

                work.ImageUrl = result.SecureUrl.ToString();
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveredWork(int id)
        {
            var work = await _context.DeliveredWorks.FindAsync(id);
            if (work == null)
                return NotFound();

            _context.DeliveredWorks.Remove(work);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

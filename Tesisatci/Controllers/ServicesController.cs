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
    public class ServicesController : ControllerBase
    {
        private readonly TesisatciDbContext _context;
        private readonly PhotoService _photoService;

        public ServicesController(TesisatciDbContext context, PhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServices()
        {
            return await _context.Services
                .Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description,
                    ImageUrl = s.ImageUrl
                }).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ServiceDto>> CreateService([FromForm] ServiceCreateDto dto)
        {
            var result = await _photoService.AddPhotoAsync(dto.Image);
            if (result.Error != null)
                return BadRequest(result.Error.Message);

            var service = new Service
            {
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = result.SecureUrl.ToString()
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServices), new { id = service.Id }, new ServiceDto
            {
                Id = service.Id,
                Title = service.Title,
                Description = service.Description,
                ImageUrl = service.ImageUrl
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
                return NotFound();

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

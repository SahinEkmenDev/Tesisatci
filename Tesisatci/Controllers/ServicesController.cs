using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tesisatci.Data;
using Tesisatci.Dtos;
using Tesisatci.Models;
using Tesisatci.Services;
using Microsoft.AspNetCore.Http;

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
                .Include(s => s.Images)
                .Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description,
                    ImageUrls = s.Images.Select(i => i.Url).ToList()
                })
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ServiceDto>> CreateService([FromForm] ServiceCreateDto dto)
        {
            if (dto.Images == null || !dto.Images.Any())
                return BadRequest("En az bir fotoğraf yükleyin.");

            var service = new Service
            {
                Title = dto.Title,
                Description = dto.Description
            };

            foreach (var image in dto.Images)
            {
                var result = await _photoService.AddPhotoAsync(image);
                if (result.Error != null)
                    return BadRequest(result.Error.Message);

                service.Images.Add(new ServiceImage
                {
                    Url = result.SecureUrl.ToString()
                });
            }

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServices), new { id = service.Id }, new ServiceDto
            {
                Id = service.Id,
                Title = service.Title,
                Description = service.Description,
                ImageUrls = service.Images.Select(i => i.Url).ToList()
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

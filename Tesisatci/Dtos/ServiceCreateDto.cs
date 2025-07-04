using Microsoft.AspNetCore.Http;

namespace Tesisatci.Dtos
{
    public class ServiceCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}

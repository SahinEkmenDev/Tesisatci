using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Tesisatci.Dtos
{
    public class ServiceCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}

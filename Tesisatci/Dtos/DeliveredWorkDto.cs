using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Tesisatci.Dtos
{
    public class DeliveredWorkDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<IFormFile> Images { get; set; } // ⭐️ çoklu dosya
    }
    public class DeliveredWorkUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<IFormFile> Images { get; set; } // ⭐️ çoklu dosya, opsiyonel
    }



}

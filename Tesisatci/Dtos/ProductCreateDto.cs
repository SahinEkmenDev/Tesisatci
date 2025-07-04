using Microsoft.AspNetCore.Http;

namespace Tesisatci.Dtos
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; } // ⭐️ EKLEDİK
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public int Stock { get; set; }
        public int CriticalStock { get; set; }
        public int CategoryId { get; set; }
        public IFormFile Image { get; set; } // ✅ tekli fotoğraf
    }

}

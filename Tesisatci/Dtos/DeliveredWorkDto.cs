using Microsoft.AspNetCore.Http;

namespace Tesisatci.Dtos
{
    public class DeliveredWorkDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
    public class DeliveredWorkUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; } // Opsiyonel: yeni resim yükleme
    }



}

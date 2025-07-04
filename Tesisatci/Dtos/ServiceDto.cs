namespace Tesisatci.Dtos
{
    public class ServiceDto
    {
        public int Id { get; set; }           // ⭐️ EKLE
        public string Title { get; set; }     // ⭐️ EKLE
        public string Description { get; set; } // ⭐️ EKLE
        public List<string> ImageUrls { get; set; }
    }
}

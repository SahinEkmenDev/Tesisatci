namespace Tesisatci.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ServiceImage> Images { get; set; } = new List<ServiceImage>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

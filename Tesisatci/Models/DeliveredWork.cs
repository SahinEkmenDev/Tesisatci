namespace Tesisatci.Models
{
    public class DeliveredWork
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; } // Cloudinary URL
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

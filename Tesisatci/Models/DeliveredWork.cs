namespace Tesisatci.Models
{
    public class DeliveredWork
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<DeliveredWorkImage> Images { get; set; } = new List<DeliveredWorkImage>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

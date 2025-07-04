namespace Tesisatci.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

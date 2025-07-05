namespace Tesisatci.Models
{
    public class DeliveredWorkImage
    {
        public int Id { get; set; }
        public string Url { get; set; } // Cloudinary URL
        public int DeliveredWorkId { get; set; }
        
        public DeliveredWork DeliveredWork { get; set; }
        

    }
}

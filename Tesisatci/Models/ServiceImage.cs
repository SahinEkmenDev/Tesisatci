namespace Tesisatci.Models
{
    public class ServiceImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}

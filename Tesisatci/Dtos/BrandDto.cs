namespace Tesisatci.Dtos
{
    public class BrandDto
    {
       
        public string Name { get; set; }
        public int CategoryId { get; set; }
    }

    public class BrandUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
    }
}

namespace Tesisatci.Dtos
{
    public class CategoryTreeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CategoryTreeDto> SubCategories { get; set; }
    }
}

namespace Tesisatci.Dtos
{
    public class ProductDetailDto
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public int Stock { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Price { get; set; }
        public double ProfitMargin { get; set; }
        public string Status { get; set; }
    }
}

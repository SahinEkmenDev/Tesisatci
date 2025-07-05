namespace Tesisatci.Dtos
{
    public class SalesCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
    }
}

namespace Tesisatci.Dtos
{
    public class StockDashboardDto
    {
        // Üst Kartlar
        public int TotalProducts { get; set; }
        public int TotalStock { get; set; }
        public decimal TotalCostValue { get; set; }
        public decimal TotalSaleValue { get; set; }
        public int CriticalStockCount { get; set; }

        // 1 Aylık Satış Özeti
        public int OneMonthSalesCount { get; set; }
        public decimal OneMonthRevenue { get; set; }
        public decimal OneMonthCost { get; set; }
        public decimal OneMonthProfit { get; set; }

        // Ürün Detayları
        public List<ProductDetailDto> ProductDetails { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tesisatci.Data;

namespace Tesisatci.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly TesisatciDbContext _context;

        public DashboardController(TesisatciDbContext context)
        {
            _context = context;
        }

        // GET: api/dashboard/summary
        [HttpGet("summary")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var totalProducts = await _context.Products.CountAsync();
            var totalSales = await _context.Sales.SumAsync(s => s.SalePrice * s.Quantity);
            var totalDeliveredWorks = await _context.DeliveredWorks.CountAsync();

            return Ok(new
            {
                TotalProducts = totalProducts,
                TotalSales = totalSales,
                TotalDeliveredWorks = totalDeliveredWorks
            });
        }

        // GET: api/dashboard/monthly-sales
        [HttpGet("monthly-sales")]
        public async Task<IActionResult> GetMonthlySales()
        {
            var last12Months = Enumerable.Range(0, 12)
                .Select(i => DateTime.UtcNow.AddMonths(-i))
                .Select(d => new
                {
                    Year = d.Year,
                    Month = d.Month
                })
                .ToList();

            var monthlySales = await _context.Sales
                .GroupBy(s => new { s.SaleDate.Year, s.SaleDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Total = g.Sum(s => s.SalePrice * s.Quantity)
                })
                .ToListAsync();

            var result = last12Months
                .Select(d => new
                {
                    d.Year,
                    d.Month,
                    Total = monthlySales.FirstOrDefault(m => m.Year == d.Year && m.Month == d.Month)?.Total ?? 0
                })
                .OrderBy(d => d.Year).ThenBy(d => d.Month)
                .ToList();

            return Ok(result);
        }
        // GET: api/dashboard/stock-dashboard
        [HttpGet("stock-dashboard")]
        public async Task<IActionResult> GetStockDashboard()
        {
            // Üst Kartlar
            var totalProducts = await _context.Products.CountAsync();
            var totalStock = await _context.Products.SumAsync(p => p.Stock);
            var totalCostValue = await _context.Products.SumAsync(p => p.CostPrice * p.Stock);
            var totalSaleValue = await _context.Products.SumAsync(p => p.Price * p.Stock);
            var criticalStockCount = await _context.Products.CountAsync(p => p.Stock < p.CriticalStock);

            // 1 Aylık Satış Özeti
            var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);
            var sales = await _context.Sales
                .Where(s => s.CreatedAt >= oneMonthAgo)
                .ToListAsync();

            var totalSalesCount = sales.Sum(s => s.Quantity);
            var totalRevenue = sales.Sum(s => s.Quantity * s.SalePrice);
            var totalCost = sales.Sum(s => s.Quantity * s.CostPrice);
            var totalProfit = totalRevenue - totalCost;

            // Ürün Detayları
            var productDetails = await _context.Products
                .Include(p => p.Category)
               
                .Select(p => new
                {
                    p.Name,
                    Category = p.Category.Name,
                   
                    p.Stock,
                    p.CostPrice,
                    p.Price,
                    ProfitMargin = p.CostPrice > 0 ? Math.Round((p.Price - p.CostPrice) / p.CostPrice * 100, 1) : 0,
                    Status = p.Stock >= p.CriticalStock ? "Normal" : "Kritik"
                }).ToListAsync();

            return Ok(new
            {
                TotalProducts = totalProducts,
                TotalStock = totalStock,
                TotalCostValue = totalCostValue,
                TotalSaleValue = totalSaleValue,
                CriticalStockCount = criticalStockCount,
                OneMonthSalesCount = totalSalesCount,
                OneMonthRevenue = totalRevenue,
                OneMonthCost = totalCost,
                OneMonthProfit = totalProfit,
                ProductDetails = productDetails
            });
        }

    }
}

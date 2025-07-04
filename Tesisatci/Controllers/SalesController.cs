using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tesisatci.Data;
using Tesisatci.Models;

namespace Tesisatci.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly TesisatciDbContext _context;

        public SalesController(TesisatciDbContext context)
        {
            _context = context;
        }

        // GET: api/sales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSales()
        {
            return await _context.Sales
                .Include(s => s.Product)
                .ToListAsync();
        }


        // POST: api/sales
        [HttpPost]
        public async Task<ActionResult<Sale>> CreateSale(Sale sale)
        {
            sale.SaleDate = DateTime.UtcNow;

            var product = await _context.Products.FindAsync(sale.ProductId);
            if (product == null)
                return BadRequest("Product not found");

            // Stok düş
            product.Stock -= sale.Quantity;
            if (product.Stock < 0) product.Stock = 0;

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSales), new { id = sale.Id }, sale);
        }

        // GET: api/sales/top-sold
        [HttpGet("top-sold")]
        public async Task<ActionResult<IEnumerable<object>>> GetTopSoldProducts()
        {
            var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);
            var topSold = await _context.Sales
                .Where(s => s.SaleDate >= oneMonthAgo)
                .GroupBy(s => s.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.Name,
                    TotalSold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(5)
                .ToListAsync();

            return topSold;
        }

        // GET: api/sales/never-sold
        [HttpGet("never-sold")]
        public async Task<ActionResult<IEnumerable<Product>>> GetNeverSoldProducts()
        {
            var soldProductIds = await _context.Sales.Select(s => s.ProductId).Distinct().ToListAsync();
            var neverSold = await _context.Products
                .Where(p => !soldProductIds.Contains(p.Id))
                .ToListAsync();

            return neverSold;
        }

        // GET: api/sales/critical-stock
        [HttpGet("critical-stock")]
        public async Task<ActionResult<IEnumerable<Product>>> GetCriticalStockProducts()
        {
            var critical = await _context.Products
                .Where(p => p.Stock <= p.CriticalStock)
                .ToListAsync();

            return critical;
        }
    }
}

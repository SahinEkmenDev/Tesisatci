using Microsoft.EntityFrameworkCore;
using Tesisatci.Models;

namespace Tesisatci.Data
{
    public class TesisatciDbContext : DbContext
    {
        public TesisatciDbContext(DbContextOptions<TesisatciDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
      
        public DbSet<Sale> Sales { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<DeliveredWork> DeliveredWorks { get; set; }
        public DbSet<Service> Services { get; set; }


    }
}

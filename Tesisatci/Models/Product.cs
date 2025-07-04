﻿namespace Tesisatci.Models
{
    public class Product
    {
        public string Unit { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public int Stock { get; set; }
        public int CriticalStock { get; set; }
        public bool IsActive { get; set; }
        public bool ShowPrice { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        
      
        public ICollection<ProductImage> Images { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

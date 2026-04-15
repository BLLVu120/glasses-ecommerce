using System;
using System.Collections.Generic;

namespace OpticalStore.DAL.Entities
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public decimal BasePrice { get; set; }
        public string ProductType { get; set; } = null!;

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    }
}

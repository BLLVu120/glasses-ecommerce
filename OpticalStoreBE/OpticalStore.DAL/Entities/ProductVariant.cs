using System;

namespace OpticalStore.DAL.Entities
{
    public class ProductVariant
    {
        public long Id { get; set; }
        public long ProductId { get; set; }

        public string? VariantType { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public string? Material { get; set; }

        public decimal PriceAdjust { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }

        public DateTime CreatedAt { get; set; }

        public Product? Product { get; set; }
    }
}

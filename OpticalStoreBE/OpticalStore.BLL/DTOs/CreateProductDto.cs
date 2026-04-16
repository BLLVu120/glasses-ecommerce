using System;

namespace OpticalStore.BLL.DTOs
{
    public class CreateProductDto
    {
        public string Name { get; set; } = null!;
        public string? Brand { get; set; }
        public string Category { get; set; } = null!;
        public decimal? WeightGram { get; set; }
        public string? ModelUrl { get; set; }
    }
}

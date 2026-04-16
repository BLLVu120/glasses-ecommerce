using System.ComponentModel.DataAnnotations;

namespace OpticalStore.API.Requests.Products
{
    public class CreateProductRequest
    {
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        [StringLength(150)]
        public string? Brand { get; set; }

        [Required]
        [RegularExpression("^(FRAME|LENS|ACCESSORY)$")]
        public string Category { get; set; } = null!;

        [Range(0, 9999.99)]
        public decimal? WeightGram { get; set; }

        [StringLength(500)]
        public string? ModelUrl { get; set; }
    }
}

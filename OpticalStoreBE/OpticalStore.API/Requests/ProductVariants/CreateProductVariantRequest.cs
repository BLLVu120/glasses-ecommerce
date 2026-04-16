using System.ComponentModel.DataAnnotations;

namespace OpticalStore.API.Requests.ProductVariants
{
    public class CreateProductVariantRequest
    {
        [Required]
        [StringLength(255)]
        [RegularExpression("^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$")]
        public string ProductId { get; set; } = null!;

        [StringLength(100)]
        public string? ColorName { get; set; }

        [StringLength(50)]
        public string? SizeLabel { get; set; }

        [Range(0, 9999.99)]
        public decimal? BridgeWidthMm { get; set; }

        [Range(0, 9999.99)]
        public decimal? LensWidthMm { get; set; }

        [Range(0, 9999.99)]
        public decimal? TempleLengthMm { get; set; }

        [StringLength(100)]
        public string? FrameFinish { get; set; }

        [Range(typeof(decimal), "0", "9999999999.99")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [RegularExpression("^(ACTIVE|INACTIVE)$")]
        public string Status { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string OrderItemType { get; set; } = null!;
    }
}

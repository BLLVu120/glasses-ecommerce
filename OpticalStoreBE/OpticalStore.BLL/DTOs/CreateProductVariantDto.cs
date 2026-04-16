namespace OpticalStore.BLL.DTOs
{
    public class CreateProductVariantDto
    {
        public string ProductId { get; set; } = null!;
        public string? ColorName { get; set; }
        public string? SizeLabel { get; set; }
        public decimal? BridgeWidthMm { get; set; }
        public decimal? LensWidthMm { get; set; }
        public decimal? TempleLengthMm { get; set; }
        public string? FrameFinish { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } = null!;
        public string OrderItemType { get; set; } = null!;
    }
}

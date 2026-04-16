using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace OpticalStore.API.Requests.ProductVariants
{
    public class GetProductVariantByIdRequest
    {
        [FromRoute(Name = "id")]
        [Required]
        [StringLength(255)]
        [RegularExpression("^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$")]
        public string Id { get; set; } = null!;
    }
}

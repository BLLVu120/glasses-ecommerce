using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpticalStore.API.Requests.ProductVariants;
using OpticalStore.API.Responses.ProductVariants;
using OpticalStore.BLL.DTOs;
using OpticalStore.BLL.Services.Interfaces;

namespace OpticalStore.API.Controllers
{
    [ApiController]
    [Route("product-variants")]
    [Tags("3. Product Variants")]
    public class ProductVariantsController : ControllerBase
    {
        private readonly IProductVariantService _productVariantService;

        public ProductVariantsController(IProductVariantService productVariantService)
        {
            _productVariantService = productVariantService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVariantResponse>>> GetAll()
        {
            var items = await _productVariantService.GetAllAsync();
            var response = new List<ProductVariantResponse>();
            foreach (var item in items)
            {
                response.Add(ToResponse(item));
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductVariantResponse>> GetById([FromRoute] GetProductVariantByIdRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _productVariantService.GetByIdAsync(request.Id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(ToResponse(item));
        }

        [HttpPost]
        public async Task<ActionResult<ProductVariantResponse>> Create([FromBody] CreateProductVariantRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dto = new CreateProductVariantDto
            {
                ProductId = request.ProductId,
                ColorName = request.ColorName,
                SizeLabel = request.SizeLabel,
                BridgeWidthMm = request.BridgeWidthMm,
                LensWidthMm = request.LensWidthMm,
                TempleLengthMm = request.TempleLengthMm,
                FrameFinish = request.FrameFinish,
                Price = request.Price,
                Quantity = request.Quantity,
                Status = request.Status,
                OrderItemType = request.OrderItemType
            };

            var created = await _productVariantService.CreateAsync(dto);
            var response = ToResponse(created);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        private static ProductVariantResponse ToResponse(ProductVariantDto dto)
        {
            return new ProductVariantResponse
            {
                Id = dto.Id,
                ProductId = dto.ProductId,
                ColorName = dto.ColorName,
                SizeLabel = dto.SizeLabel,
                BridgeWidthMm = dto.BridgeWidthMm,
                LensWidthMm = dto.LensWidthMm,
                TempleLengthMm = dto.TempleLengthMm,
                FrameFinish = dto.FrameFinish,
                Price = dto.Price,
                Quantity = dto.Quantity,
                Status = dto.Status,
                OrderItemType = dto.OrderItemType
            };
        }
    }
}

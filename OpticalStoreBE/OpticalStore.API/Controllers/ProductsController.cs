using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpticalStore.API.Requests.Products;
using OpticalStore.API.Responses.Products;
using OpticalStore.BLL.DTOs;
using OpticalStore.BLL.Services.Interfaces;

namespace OpticalStore.API.Controllers
{
    [ApiController]
    [Route("products")]
    [Tags("2. Products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll()
        {
            var items = await _productService.GetAllAsync();
            var resp = new List<ProductResponse>();
            foreach (var p in items)
            {
                resp.Add(new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Brand = p.Brand,
                    Category = p.Category,
                    WeightGram = p.WeightGram,
                    Status = p.Status,
                    ModelUrl = p.ModelUrl
                });
            }
            return Ok(resp);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Product id is required.");
            }

            if (!Guid.TryParse(id, out _))
            {
                return BadRequest("Product id is not a valid GUID.");
            }

            var p = await _productService.GetByIdAsync(id);
            if (p == null) return NotFound();
            var resp = new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Brand = p.Brand,
                Category = p.Category,
                WeightGram = p.WeightGram,
                Status = p.Status,
                ModelUrl = p.ModelUrl
            };
            return Ok(resp);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponse>> Create([FromBody] CreateProductRequest? req)
        {
            if (req == null)
            {
                return BadRequest("Request body is required.");
            }

            var dto = new CreateProductDto
            {
                Name = req.Name,
                Brand = req.Brand,
                Category = req.Category,
                WeightGram = req.WeightGram,
                ModelUrl = req.ModelUrl
            };

            var created = await _productService.CreateAsync(dto);

            var resp = new ProductResponse
            {
                Id = created.Id,
                Name = created.Name,
                Brand = created.Brand,
                Category = created.Category,
                WeightGram = created.WeightGram,
                Status = created.Status,
                ModelUrl = created.ModelUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = resp.Id }, resp);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpticalStore.BLL.DTOs;
using OpticalStore.BLL.Services.Interfaces;
using OpticalStore.DAL.Entities;
using OpticalStore.DAL.Entities.Enums;
using OpticalStore.DAL.Repositories.Interfaces;

namespace OpticalStore.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => new ProductDto
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

        public async Task<ProductDto?> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Product id is required.");
            }

            var p = await _productRepository.GetByIdAsync(id);
            if (p == null) return null;
            return new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Brand = p.Brand,
                Category = p.Category,
                WeightGram = p.WeightGram,
                Status = p.Status,
                ModelUrl = p.ModelUrl
            };
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("Product name is required.");
            }

            if (dto.Name.Length > 255)
            {
                throw new ArgumentException("Product name must be 255 characters or fewer.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Brand) && dto.Brand.Length > 150)
            {
                throw new ArgumentException("Brand must be 150 characters or fewer.");
            }

            if (string.IsNullOrWhiteSpace(dto.Category))
            {
                throw new ArgumentException("Product category is required.");
            }

            var normalizedCategory = dto.Category.Trim().ToUpperInvariant();
            var isValidCategory = normalizedCategory == ProductCategoryValues.Frame
                || normalizedCategory == ProductCategoryValues.Lens
                || normalizedCategory == ProductCategoryValues.Accessory;

            if (!isValidCategory)
            {
                throw new ArgumentException("Category must be FRAME, LENS, or ACCESSORY.");
            }

            if (dto.WeightGram.HasValue && dto.WeightGram.Value < 0)
            {
                throw new ArgumentException("WeightGram cannot be negative.");
            }

            if (dto.WeightGram.HasValue && dto.WeightGram.Value > 9999.99m)
            {
                throw new ArgumentException("WeightGram exceeds allowed range.");
            }

            if (!string.IsNullOrWhiteSpace(dto.ModelUrl) && dto.ModelUrl.Length > 500)
            {
                throw new ArgumentException("ModelUrl must be 500 characters or fewer.");
            }

            var entity = new Product
            {
                Id = Guid.NewGuid().ToString("D").ToLowerInvariant(),
                Name = dto.Name.Trim(),
                Brand = string.IsNullOrWhiteSpace(dto.Brand) ? null : dto.Brand.Trim(),
                Category = normalizedCategory,
                WeightGram = dto.WeightGram,
                ModelUrl = string.IsNullOrWhiteSpace(dto.ModelUrl) ? null : dto.ModelUrl.Trim(),
                Status = StatusValues.Active
            };

            await _productRepository.AddAsync(entity);
            await _productRepository.SaveChangesAsync();

            return new ProductDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Brand = entity.Brand,
                Category = entity.Category,
                WeightGram = entity.WeightGram,
                Status = entity.Status,
                ModelUrl = entity.ModelUrl
            };
        }
    }
}

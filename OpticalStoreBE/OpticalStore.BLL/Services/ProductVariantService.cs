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
    public class ProductVariantService : IProductVariantService
    {
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IProductRepository _productRepository;

        public ProductVariantService(IProductVariantRepository productVariantRepository, IProductRepository productRepository)
        {
            _productVariantRepository = productVariantRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductVariantDto>> GetAllAsync()
        {
            var variants = await _productVariantRepository.GetAllAsync();
            return variants.Select(ToDto);
        }

        public async Task<ProductVariantDto?> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Product variant id is required.");
            }

            var variant = await _productVariantRepository.GetByIdAsync(id);
            if (variant == null)
            {
                return null;
            }

            return ToDto(variant);
        }

        public async Task<ProductVariantDto> CreateAsync(CreateProductVariantDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.ProductId))
            {
                throw new ArgumentException("ProductId is required.");
            }

            var normalizedProductId = dto.ProductId.Trim().ToLowerInvariant();
            if (!Guid.TryParse(normalizedProductId, out _))
            {
                throw new ArgumentException("ProductId must be a valid GUID.");
            }

            if (dto.Price < 0)
            {
                throw new ArgumentException("Price cannot be negative.");
            }

            if (dto.Quantity < 0)
            {
                throw new ArgumentException("Quantity cannot be negative.");
            }

            if (string.IsNullOrWhiteSpace(dto.OrderItemType))
            {
                throw new ArgumentException("OrderItemType is required.");
            }

            if (dto.OrderItemType.Length > 100)
            {
                throw new ArgumentException("OrderItemType must be 100 characters or fewer.");
            }

            if (!string.IsNullOrWhiteSpace(dto.ColorName) && dto.ColorName.Length > 100)
            {
                throw new ArgumentException("ColorName must be 100 characters or fewer.");
            }

            if (!string.IsNullOrWhiteSpace(dto.SizeLabel) && dto.SizeLabel.Length > 50)
            {
                throw new ArgumentException("SizeLabel must be 50 characters or fewer.");
            }

            if (!string.IsNullOrWhiteSpace(dto.FrameFinish) && dto.FrameFinish.Length > 100)
            {
                throw new ArgumentException("FrameFinish must be 100 characters or fewer.");
            }

            ValidateDimension(dto.BridgeWidthMm, nameof(dto.BridgeWidthMm));
            ValidateDimension(dto.LensWidthMm, nameof(dto.LensWidthMm));
            ValidateDimension(dto.TempleLengthMm, nameof(dto.TempleLengthMm));

            var normalizedStatus = dto.Status?.Trim().ToUpperInvariant();
            if (string.IsNullOrWhiteSpace(normalizedStatus))
            {
                throw new ArgumentException("Status is required.");
            }

            if (normalizedStatus != StatusValues.Active && normalizedStatus != StatusValues.Inactive)
            {
                throw new ArgumentException("Status must be ACTIVE or INACTIVE.");
            }

            var existingProduct = await _productRepository.GetByIdAsync(normalizedProductId);
            if (existingProduct == null)
            {
                throw new ArgumentException("ProductId does not exist.");
            }

            var entity = new ProductVariant
            {
                Id = Guid.NewGuid().ToString("D").ToLowerInvariant(),
                ProductId = normalizedProductId,
                ColorName = string.IsNullOrWhiteSpace(dto.ColorName) ? null : dto.ColorName.Trim(),
                SizeLabel = string.IsNullOrWhiteSpace(dto.SizeLabel) ? null : dto.SizeLabel.Trim(),
                BridgeWidthMm = dto.BridgeWidthMm,
                LensWidthMm = dto.LensWidthMm,
                TempleLengthMm = dto.TempleLengthMm,
                FrameFinish = string.IsNullOrWhiteSpace(dto.FrameFinish) ? null : dto.FrameFinish.Trim(),
                Price = dto.Price,
                Quantity = dto.Quantity,
                Status = normalizedStatus,
                OrderItemType = dto.OrderItemType.Trim()
            };

            await _productVariantRepository.AddAsync(entity);
            await _productVariantRepository.SaveChangesAsync();

            return ToDto(entity);
        }

        private static void ValidateDimension(decimal? value, string fieldName)
        {
            if (!value.HasValue)
            {
                return;
            }

            if (value.Value < 0)
            {
                throw new ArgumentException($"{fieldName} cannot be negative.");
            }

            if (value.Value > 9999.99m)
            {
                throw new ArgumentException($"{fieldName} exceeds allowed range.");
            }
        }

        private static ProductVariantDto ToDto(ProductVariant variant)
        {
            return new ProductVariantDto
            {
                Id = variant.Id,
                ProductId = variant.ProductId,
                ColorName = variant.ColorName,
                SizeLabel = variant.SizeLabel,
                BridgeWidthMm = variant.BridgeWidthMm,
                LensWidthMm = variant.LensWidthMm,
                TempleLengthMm = variant.TempleLengthMm,
                FrameFinish = variant.FrameFinish,
                Price = variant.Price,
                Quantity = variant.Quantity,
                Status = variant.Status,
                OrderItemType = variant.OrderItemType
            };
        }
    }
}

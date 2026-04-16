using System.Collections.Generic;
using System.Threading.Tasks;
using OpticalStore.BLL.DTOs;

namespace OpticalStore.BLL.Services.Interfaces
{
    public interface IProductVariantService
    {
        Task<IEnumerable<ProductVariantDto>> GetAllAsync();
        Task<ProductVariantDto?> GetByIdAsync(string id);
        Task<ProductVariantDto> CreateAsync(CreateProductVariantDto dto);
    }
}

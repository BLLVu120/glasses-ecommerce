using System.Collections.Generic;
using System.Threading.Tasks;
using OpticalStore.BLL.DTOs;

namespace OpticalStore.BLL.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(string id);
        Task<ProductDto> CreateAsync(CreateProductDto dto);
    }
}

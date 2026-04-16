using System.Collections.Generic;
using System.Threading.Tasks;
using OpticalStore.DAL.Entities;

namespace OpticalStore.DAL.Repositories.Interfaces
{
    public interface IProductVariantRepository
    {
        Task<IEnumerable<ProductVariant>> GetAllAsync();
        Task<ProductVariant?> GetByIdAsync(string id);
        Task AddAsync(ProductVariant variant);
        Task SaveChangesAsync();
    }
}

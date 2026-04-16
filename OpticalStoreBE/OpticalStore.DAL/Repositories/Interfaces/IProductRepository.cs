using System.Collections.Generic;
using System.Threading.Tasks;
using OpticalStore.DAL.Entities;

namespace OpticalStore.DAL.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(string id);
        Task AddAsync(Product product);
        Task SaveChangesAsync();
    }
}

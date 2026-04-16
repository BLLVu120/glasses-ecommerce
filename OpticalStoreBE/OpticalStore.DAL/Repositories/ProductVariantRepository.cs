using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpticalStore.DAL.DBContext;
using OpticalStore.DAL.Entities;
using OpticalStore.DAL.Repositories.Interfaces;

namespace OpticalStore.DAL.Repositories
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly OpticalStoreDbContext _db;

        public ProductVariantRepository(OpticalStoreDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ProductVariant>> GetAllAsync()
        {
            return await _db.ProductVariants.AsNoTracking().ToListAsync();
        }

        public async Task<ProductVariant?> GetByIdAsync(string id)
        {
            return await _db.ProductVariants.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(ProductVariant variant)
        {
            await _db.ProductVariants.AddAsync(variant);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

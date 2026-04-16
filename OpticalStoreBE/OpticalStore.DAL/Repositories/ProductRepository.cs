using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpticalStore.DAL.DBContext;
using OpticalStore.DAL.Entities;
using OpticalStore.DAL.Repositories.Interfaces;

namespace OpticalStore.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly OpticalStoreDbContext _db;

        public ProductRepository(OpticalStoreDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            return await _db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            await _db.Products.AddAsync(product);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

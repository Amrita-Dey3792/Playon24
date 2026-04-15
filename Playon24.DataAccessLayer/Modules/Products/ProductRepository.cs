using Microsoft.EntityFrameworkCore;
using Playon24.DataAccessLayer.Data;
using Playon24.DataAccessLayer.Modules.Products.Interfaces;
using Playon24.Domain.Entities;

namespace Playon24.DataAccessLayer.Modules.Products
{

    public class ProductRepository : IProductRepository
    {
        private readonly Payon24DbContext _dbContext;

        public ProductRepository(Payon24DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> AddAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return await _dbContext.Products
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var product = await _dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }

        public async Task<bool> DeleteAsync(Product product)
        {
            _dbContext.Products.Remove(product);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task UpdateAsync(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}

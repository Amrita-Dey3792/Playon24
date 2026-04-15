using Playon24.Domain.Entities;

namespace Playon24.DataAccessLayer.Modules.Products.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product);
        Task<Product?> GetByIdAsync(int id);
        Task<IReadOnlyList<Product>> GetAllAsync();
        Task UpdateAsync(Product product);
        Task<bool> DeleteAsync(Product product);
    }
}

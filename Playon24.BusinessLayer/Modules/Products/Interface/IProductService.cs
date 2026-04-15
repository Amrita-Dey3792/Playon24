using Playon24.BusinessLayer.Modules.Files.Helpers;
using Playon24.Domain.Entities;

namespace Playon24.BusinessLayer.Modules.Products.Interface
{
    public interface IProductService
    {
        Task<Product> AddAsync(Product product, FileUploadRequest? imageUpload);
        Task<Product?> GetByIdAsync(int id);
        Task<IReadOnlyList<Product>> GetAllAsync();
        Task UpdateAsync(Product product, FileUploadRequest? imageUpload);
        Task<bool> DeleteAsync(int id);
    }
}

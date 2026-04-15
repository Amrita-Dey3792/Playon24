using Playon24.BusinessLayer.Exceptions;
using Playon24.BusinessLayer.Modules.Files.Helpers;
using Playon24.BusinessLayer.Modules.Files.Interface;
using Playon24.BusinessLayer.Modules.Products.Interface;
using Playon24.DataAccessLayer.Modules.Products.Interfaces;
using Playon24.Domain.Entities;

namespace Playon24.BusinessLayer.Modules.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IFileService _fileService;

        public ProductService(IProductRepository productRepository, IFileService fileService)
        {
            _repo = productRepository;
            _fileService = fileService;
        }

        public async Task<Product> AddAsync(Product product, FileUploadRequest? imageUpload)
        {
            var now = DateTime.UtcNow;
            NormalizeProduct(product);
            product.CreatedAt = now;
            product.UpdatedAt = now;

            string imagePath = string.Empty;
            try
            {
                if (imageUpload != null)
                    imagePath = await _fileService.UploadAsync(imageUpload, "products");
                product.ImagePath = imagePath;
                return await _repo.AddAsync(product);
            }
            catch
            {
                await _fileService.DeleteUploadedFileAsync(imagePath);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await GetProductOrThrow(id);
            var deleted = await _repo.DeleteAsync(product);
            if (deleted)
                await _fileService.DeleteUploadedFileAsync(product.ImagePath);
            return deleted;
        }

        public Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return _repo.GetAllAsync();
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Product product, FileUploadRequest? imageUpload)
        {
            var existing = await GetProductOrThrow(product.Id);
            NormalizeProduct(product);
            product.CreatedAt = existing.CreatedAt;
            product.UpdatedAt = DateTime.UtcNow;

            string newImagePath = string.Empty;
            var currentImagePath = existing.ImagePath;
            if (imageUpload != null)
            {
                newImagePath = await _fileService.UploadAsync(imageUpload, "products");
                product.ImagePath = newImagePath;
            }
            else
            {
                product.ImagePath = currentImagePath;
            }

            try
            {
                await _repo.UpdateAsync(product);
                if (!string.IsNullOrWhiteSpace(newImagePath))
                    await _fileService.DeleteUploadedFileAsync(currentImagePath);
            }
            catch
            {
                if (!string.IsNullOrWhiteSpace(newImagePath))
                    await _fileService.DeleteUploadedFileAsync(newImagePath);
                throw;
            }
        }

        private static void NormalizeProduct(Product product)
        {
            product.ProductName = product.ProductName.Trim();
            product.Description = string.IsNullOrWhiteSpace(product.Description)
                ? null
                : product.Description.Trim();
            if (product.UnitPrice < 0)
                throw new InvalidUserInputException("Unit price cannot be negative.");
            if (product.StockQuantity < 0)
                throw new InvalidUserInputException("Stock quantity cannot be negative.");
        }

        private async Task<Product> GetProductOrThrow(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                throw new InvalidOperationException("Product not found.");

            return product;
        }
    }
}

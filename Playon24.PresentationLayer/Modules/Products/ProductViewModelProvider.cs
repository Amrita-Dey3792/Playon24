using AutoMapper;
using Microsoft.AspNetCore.Http;
using Playon24.BusinessLayer.Modules.Files.Helpers;
using Playon24.BusinessLayer.Modules.Products.Interface;
using Playon24.Domain.Entities;
using Playon24.PresentationLayer.Modules.Products.Interface;
using Playon24.PresentationLayer.Modules.Products.ViewModels;

namespace Playon24.PresentationLayer.Modules.Products
{
    public class ProductViewModelProvider : IProductViewModelProvider
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductViewModelProvider(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public async Task AddAsync(ProductCreateViewModel productCvm)
        {
            var product = _mapper.Map<Product>(productCvm);
            using var upload = ToUploadRequest(productCvm.ImageFile);
            await _productService.AddAsync(product, upload);
        }

        public async Task<IReadOnlyList<ProductListViewModel>> GetAllAsync()
        {
            var products = await _productService.GetAllAsync();
            return _mapper.Map<IReadOnlyList<ProductListViewModel>>(products);
        }

        public async Task<ProductEditViewModel?> GetDetailsByIdAsync(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return product == null
                ?
                null :
                _mapper.Map<ProductEditViewModel>(product);
        }

        public async Task UpdateAsync(ProductEditViewModel productEvm)
        {
            var entity = _mapper.Map<Product>(productEvm);
            using var upload = ToUploadRequest(productEvm.ImageFile);
            await _productService.UpdateAsync(entity, upload);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _productService.DeleteAsync(id);
        }

        private static FileUploadRequest? ToUploadRequest(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            return new FileUploadRequest(
                imageFile.FileName,
                imageFile.Length,
                imageFile.OpenReadStream());
        }
    }
}
